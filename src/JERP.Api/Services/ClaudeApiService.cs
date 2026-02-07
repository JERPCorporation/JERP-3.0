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
            
            // Validate response structure
            if (!jsonResponse.RootElement.TryGetProperty("content", out var contentArray) ||
                contentArray.ValueKind != JsonValueKind.Array ||
                contentArray.GetArrayLength() == 0)
            {
                _logger.LogError("Claude API returned unexpected response format");
                return "Claude API returned an unexpected response format";
            }

            if (!contentArray[0].TryGetProperty("text", out var textElement))
            {
                _logger.LogError("Claude API response missing text property");
                return "Claude API returned an unexpected response format";
            }

            var content = textElement.GetString();
            return content ?? "No response from Claude API";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Claude API");
            return "An error occurred while processing your request with the AI assistant";
        }
    }
}
