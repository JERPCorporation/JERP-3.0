namespace JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance report data transfer object
/// </summary>
public class ComplianceReportDto
{
    public required string CompanyName { get; set; }
    public DateTime ReportDate { get; set; }
    public ComplianceScoreDto Score { get; set; } = new();
    public List<ComplianceViolationDto> Violations { get; set; } = new();
}
