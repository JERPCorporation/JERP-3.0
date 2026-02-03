using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a detailed line item in a payroll record
/// </summary>
public class PayrollRecordDetail : BaseEntity
{
    /// <summary>
    /// Foreign key to the payroll record
    /// </summary>
    [Required]
    public Guid PayrollRecordId { get; set; }

    /// <summary>
    /// Type of line item (earnings, tax, or deduction)
    /// </summary>
    [Required]
    public PayrollRecordDetailType Type { get; set; }

    /// <summary>
    /// Description of the line item
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Amount for this line item
    /// </summary>
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// Indicates if this is a year-to-date total
    /// </summary>
    public bool IsYTD { get; set; } = false;

    // Navigation properties
    public PayrollRecord PayrollRecord { get; set; } = null!;
}
