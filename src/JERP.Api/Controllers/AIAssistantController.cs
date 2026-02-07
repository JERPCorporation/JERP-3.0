using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Api.Services;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AIAssistantController : ControllerBase
{
    private readonly ClaudeApiService _claudeApiService;
    private readonly ILogger<AIAssistantController> _logger;

    public AIAssistantController(
        ClaudeApiService claudeApiService,
        ILogger<AIAssistantController> logger)
    {
        _claudeApiService = claudeApiService;
        _logger = logger;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskQuestion([FromBody] AskQuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { error = "Question is required" });
        }

        _logger.LogInformation("AI Assistant question received: {Question}", request.Question);

        try
        {
            var response = await _claudeApiService.SendMessageAsync(request.Question);
            
            return Ok(new
            {
                question = request.Question,
                answer = response,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing AI Assistant question");
            return StatusCode(500, new { error = "Error processing your question" });
        }
    }
}

public class AskQuestionRequest
{
    public string Question { get; set; } = string.Empty;
}
