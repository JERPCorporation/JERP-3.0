namespace JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance score summary
/// </summary>
public class ComplianceScoreDto
{
    public decimal Score { get; set; }
    public int TotalChecks { get; set; }
    public int TotalViolations { get; set; }
    public Dictionary<string, int> ViolationsByCategory { get; set; } = new();
}
