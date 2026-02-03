using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.Financial.Rules;

/// <summary>
/// IFRS Inventory Valuation rule: LIFO method not allowed under IFRS
/// </summary>
public class IFRSInventoryValuationRule : ComplianceRuleBase
{
    public override string RuleName => "IFRS Inventory Valuation";
    public override ViolationType ViolationType => ViolationType.FinancialCompliance;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.High;

    public IFRSInventoryValuationRule(ILogger<IFRSInventoryValuationRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not InventoryValuationContext inventoryContext)
        {
            throw new ArgumentException("Context must be an InventoryValuationContext", nameof(context));
        }

        LogEvaluationStart("Inventory", inventoryContext.InventoryId);

        var violations = new List<ComplianceViolation>();

        // IFRS does not allow LIFO (Last-In, First-Out) method
        // Only FIFO (First-In, First-Out) and Weighted Average are acceptable
        if (inventoryContext.ValuationMethod.Equals("LIFO", StringComparison.OrdinalIgnoreCase))
        {
            var details = new
            {
                InventoryId = inventoryContext.InventoryId,
                ValuationMethod = inventoryContext.ValuationMethod,
                InventoryValue = inventoryContext.InventoryValue,
                Issue = "LIFO method not permitted under IFRS"
            };

            violations.Add(CreateViolation(
                "Inventory",
                inventoryContext.InventoryId,
                "LIFO (Last-In, First-Out) inventory valuation method is not permitted under IFRS. Use FIFO or Weighted Average instead.",
                ViolationSeverity.High,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        // Verify that inventory is valued at lower of cost or net realizable value
        if (inventoryContext.InventoryValue > inventoryContext.NetRealizableValue)
        {
            var writedownAmount = inventoryContext.InventoryValue - inventoryContext.NetRealizableValue;
            var details = new
            {
                InventoryId = inventoryContext.InventoryId,
                ValuationMethod = inventoryContext.ValuationMethod,
                InventoryValue = inventoryContext.InventoryValue,
                NetRealizableValue = inventoryContext.NetRealizableValue,
                RequiredWritedown = writedownAmount,
                Issue = "Inventory not valued at lower of cost or NRV"
            };

            violations.Add(CreateViolation(
                "Inventory",
                inventoryContext.InventoryId,
                $"Inventory valued at ${inventoryContext.InventoryValue:N2} exceeds net realizable value of ${inventoryContext.NetRealizableValue:N2}. Writedown of ${writedownAmount:N2} required.",
                ViolationSeverity.Medium,
                writedownAmount,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Inventory {inventoryContext.InventoryId} complies with IFRS valuation rules")
            : ComplianceResult.NonCompliant($"Inventory {inventoryContext.InventoryId} has {violations.Count} IFRS violations", violations);
    }

    /// <summary>
    /// Context for inventory valuation evaluation
    /// </summary>
    public class InventoryValuationContext
    {
        public Guid InventoryId { get; set; }
        public string ValuationMethod { get; set; } = string.Empty;
        public decimal InventoryValue { get; set; }
        public decimal NetRealizableValue { get; set; }
    }
}
