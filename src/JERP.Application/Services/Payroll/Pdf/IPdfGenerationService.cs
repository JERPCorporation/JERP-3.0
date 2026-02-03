namespace JERP.Application.Services.Payroll.Pdf;

/// <summary>
/// Interface for PDF generation services
/// </summary>
public interface IPdfGenerationService
{
    /// <summary>
    /// Generates a pay stub PDF for a payroll record
    /// </summary>
    Task<byte[]> GeneratePayStubAsync(Guid payrollRecordId);

    /// <summary>
    /// Generates pay stubs for all records in a pay period
    /// </summary>
    Task<byte[]> GenerateBulkPayStubsAsync(Guid payPeriodId);
}
