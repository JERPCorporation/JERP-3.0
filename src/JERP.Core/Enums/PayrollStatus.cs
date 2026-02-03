namespace JERP.Core.Enums;

/// <summary>
/// Represents the status of a payroll period or record
/// </summary>
public enum PayrollStatus
{
    Draft,
    Calculated,
    Submitted,
    Approved,
    Paid,
    Rejected
}
