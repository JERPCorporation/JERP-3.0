/*
 * JERP 3.0 - Claude Service Integration
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using Anthropic.SDK;
using Anthropic.SDK.Messaging;

namespace JERP.Api.Services;

public interface IClaudeService
{
    Task<ClaudeAnalysisDto> AnalyzeClientFinancesAsync(Client client, List<Transaction> transactions);
    Task<string> CategorizeTransactionAsync(Transaction transaction);
    Task<List<string>> DetectAnomaliesAsync(List<Transaction> transactions);
    Task<TaxCalculationDto> CalculateTaxesAsync(Client client, List<Transaction> transactions);
}

public class ClaudeService : IClaudeService
{
    private readonly AnthropicClient _anthropic;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ClaudeService> _logger;

    public ClaudeService(
        IConfiguration configuration,
        ILogger<ClaudeService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        var apiKey = configuration["Anthropic:ApiKey"];
        _anthropic = new AnthropicClient(apiKey);
    }

    public async Task<ClaudeAnalysisDto> AnalyzeClientFinancesAsync(
        Client client, 
        List<Transaction> transactions)
    {
        var prompt = BuildAnalysisPrompt(client, transactions);
        
        var request = new MessageRequest
        {
            Model = "claude-sonnet-4-20250514",
            MaxTokens = 2048,
            Messages = new List<Message>
            {
                new Message
                {
                    Role = "user",
                    Content = prompt
                }
            }
        };

        try
        {
            var response = await _anthropic.Messages.CreateAsync(request);
            var content = response.Content.FirstOrDefault()?.Text ?? "";
            
            // Track cost
            var cost = CalculateCost(response.Usage);
            await TrackUsageAsync(client.Id, cost, response.Usage);
            
            return ParseAnalysis(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Claude API");
            throw;
        }
    }

    public async Task<string> CategorizeTransactionAsync(Transaction transaction)
    {
        var prompt = $@"Categoriza esta transacción para contabilidad guatemalteca:

Descripción: {transaction.Description}
Monto: Q{transaction.Amount}
Fecha: {transaction.Date:yyyy-MM-dd}

Categorías válidas:
- Ingresos por Ventas
- Costo de Ventas
- Gastos Operativos
- Gastos de Personal
- Impuestos y Contribuciones
- Activos
- Pasivos

Responde SOLO con el nombre de la categoría, nada más.";

        var request = new MessageRequest
        {
            Model = "claude-haiku-4-20250514", // Cheaper model for simple tasks
            MaxTokens = 50,
            Messages = new List<Message>
            {
                new Message { Role = "user", Content = prompt }
            }
        };

        var response = await _anthropic.Messages.CreateAsync(request);
        return response.Content.FirstOrDefault()?.Text?.Trim() ?? "Sin Categoría";
    }

    public async Task<List<string>> DetectAnomaliesAsync(List<Transaction> transactions)
    {
        var prompt = $@"Analiza estas transacciones y detecta ANOMALÍAS o ERRORES:

Transacciones:
{string.Join("\n", transactions.Select(t => 
    $"- {t.Date:yyyy-MM-dd} | {t.Description} | Q{t.Amount} | {t.Category}"))}

Busca:
1. Duplicados obvios
2. Montos inusuales (muy altos/bajos comparado con el patrón)
3. Categorías incorrectas
4. Fechas fuera de rango
5. Descripciones sospechosas

Devuelve SOLO las anomalías en formato:
- [TIPO] Descripción del problema

Si no hay anomalías, responde: NINGUNA";

        var request = new MessageRequest
        {
            Model = "claude-sonnet-4-20250514",
            MaxTokens = 1024,
            Messages = new List<Message>
            {
                new Message { Role = "user", Content = prompt }
            }
        };

        var response = await _anthropic.Messages.CreateAsync(request);
        var content = response.Content.FirstOrDefault()?.Text ?? "";
        
        if (content.Trim() == "NINGUNA")
            return new List<string>();
            
        return content.Split('\n')
            .Where(line => line.StartsWith("-"))
            .Select(line => line.TrimStart('-').Trim())
            .ToList();
    }

    public async Task<TaxCalculationDto> CalculateTaxesAsync(
        Client client, 
        List<Transaction> transactions)
    {
        var income = transactions
            .Where(t => t.Amount > 0)
            .Sum(t => t.Amount);
            
        var expenses = transactions
            .Where(t => t.Amount < 0)
            .Sum(t => Math.Abs(t.Amount));

        var prompt = $@"Calcula impuestos para Guatemala:

Cliente: {client.Name}
Régimen Fiscal: {client.TaxRegime}
Ingresos del mes: Q{income:N2}
Gastos del mes: Q{expenses:N2}

Calcula:
1. ISR (según régimen: {client.TaxRegime})
   - Si es 'Utilidades': 31% sobre utilidad
   - Si es 'Simplificado': 5% sobre ingresos
2. IVA (12% sobre ingresos)

Responde en JSON:
{{
  ""isr"": 0.00,
  ""iva"": 0.00,
  ""totalTaxes"": 0.00,
  ""explanation"": ""Explicación breve""
}}";

        var request = new MessageRequest
        {
            Model = "claude-sonnet-4-20250514",
            MaxTokens = 512,
            Messages = new List<Message>
            {
                new Message { Role = "user", Content = prompt }
            }
        };

        var response = await _anthropic.Messages.CreateAsync(request);
        var content = response.Content.FirstOrDefault()?.Text ?? "";
        
        return ParseTaxCalculation(content);
    }

    private string BuildAnalysisPrompt(Client client, List<Transaction> transactions)
    {
        return $@"Actúas como ASISTENTE de contador profesional en Guatemala.

CLIENTE:
- Nombre: {client.Name}
- Negocio: {client.BusinessType}
- Régimen: {client.TaxRegime}

TRANSACCIONES DEL MES ({transactions.Count}):
{string.Join("\n", transactions.Take(100).Select(t => 
    $"- {t.Date:yyyy-MM-dd} | {t.Description} | Q{t.Amount:N2} | {t.Category}"))}

ANALIZA y proporciona:

1. ALERTAS CRÍTICAS:
   - Errores obvios
   - Duplicados
   - Montos inusuales

2. SUGERENCIAS:
   - Deducciones fiscales disponibles
   - Categorías para revisar
   - Optimizaciones

3. CÁLCULOS PRELIMINARES:
   - ISR estimado
   - IVA
   - Totales

4. RECORDATORIOS:
   - Deadlines SAT
   - Documentos faltantes

Responde en JSON:
{{
  ""alerts"": [{{""type"": ""error|warning"", ""message"": ""...""}}],
  ""suggestions"": [{{""category"": ""..."", ""message"": ""...""}}],
  ""calculations"": {{""isr"": 0, ""iva"": 0, ""income"": 0, ""expenses"": 0}},
  ""reminders"": [""...""],
  ""summary"": ""Resumen para el contador en 2-3 oraciones""
}}

IMPORTANTE: Marca todo como PRELIMINAR - requiere revisión del contador.";
    }

    private ClaudeAnalysisDto ParseAnalysis(string content)
    {
        try
        {
            var cleaned = content
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();
                
            return System.Text.Json.JsonSerializer.Deserialize<ClaudeAnalysisDto>(cleaned)
                ?? new ClaudeAnalysisDto();
        }
        catch
        {
            return new ClaudeAnalysisDto
            {
                Summary = content.Substring(0, Math.Min(200, content.Length))
            };
        }
    }

    private TaxCalculationDto ParseTaxCalculation(string content)
    {
        try
        {
            var cleaned = content.Replace("```json", "").Replace("```", "").Trim();
            return System.Text.Json.JsonSerializer.Deserialize<TaxCalculationDto>(cleaned)
                ?? new TaxCalculationDto();
        }
        catch
        {
            return new TaxCalculationDto();
        }
    }

    private decimal CalculateCost(Usage usage)
    {
        // Claude pricing (approximate)
        var inputCost = (usage.InputTokens / 1_000_000m) * 3m; // $3 per 1M
        var outputCost = (usage.OutputTokens / 1_000_000m) * 15m; // $15 per 1M
        return inputCost + outputCost;
    }

    private async Task TrackUsageAsync(Guid clientId, decimal cost, Usage usage)
    {
        _logger.LogInformation(
            "Claude API usage - Client: {ClientId}, Cost: ${Cost:F4}, Tokens: {Input}/{Output}",
            clientId, cost, usage.InputTokens, usage.OutputTokens
        );
        
        // TODO: Save to database for billing
    }
}

// DTOs
public class ClaudeAnalysisDto
{
    public List<Alert> Alerts { get; set; } = new();
    public List<Suggestion> Suggestions { get; set; } = new();
    public Calculations Calculations { get; set; } = new();
    public List<string> Reminders { get; set; } = new();
    public string Summary { get; set; } = string.Empty;

    public class Alert
    {
        public string Type { get; set; } = string.Empty; // error, warning
        public string Message { get; set; } = string.Empty;
    }

    public class Suggestion
    {
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class Calculations
    {
        public decimal Isr { get; set; }
        public decimal Iva { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }
}

public class TaxCalculationDto
{
    public decimal Isr { get; set; }
    public decimal Iva { get; set; }
    public decimal TotalTaxes { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
