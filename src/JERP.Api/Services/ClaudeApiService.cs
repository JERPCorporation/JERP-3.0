using System.Text;
using System.Text.Json;

namespace JERP.Api.Services;

public class ClaudeApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ClaudeApiService> _logger;

    public ClaudeApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ClaudeApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = _configuration["Claude:ApiKey"];
            var apiUrl = _configuration["Claude:ApiUrl"];
            var model = _configuration["Claude:Model"];

            if (string.IsNullOrEmpty(apiKey) || apiKey == "OVERRIDE_IN_ENVIRONMENT")
            {
                _logger.LogWarning("Claude API key not configured");
                return "Claude API is not configured. Please set the API key in environment variables.";
            }

            var requestBody = new
            {
                model = model,
                max_tokens = 1024,
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("x-api-key", apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var jsonResponse = JsonDocument.Parse(responseContent);
            
            var content = jsonResponse.RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString();

            return content ?? "No response from Claude API";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Claude API");
            return $"Error: {ex.Message}";
        }
    }
}
