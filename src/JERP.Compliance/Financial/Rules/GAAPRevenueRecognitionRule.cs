/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.Financial.Rules;

/// <summary>
/// GAAP Revenue Recognition rule: ASC 606 5-step model compliance
/// </summary>
public class GAAPRevenueRecognitionRule : ComplianceRuleBase
{
    public override string RuleName => "GAAP Revenue Recognition (ASC 606)";
    public override ViolationType ViolationType => ViolationType.FinancialCompliance;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.High;

    public GAAPRevenueRecognitionRule(ILogger<GAAPRevenueRecognitionRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not RevenueRecognitionContext revenueContext)
        {
            throw new ArgumentException("Context must be a RevenueRecognitionContext", nameof(context));
        }

        LogEvaluationStart("Revenue", revenueContext.TransactionId);

        var violations = new List<ComplianceViolation>();

        // ASC 606 Five-Step Model:
        // 1. Identify the contract with a customer
        // 2. Identify the performance obligations in the contract
        // 3. Determine the transaction price
        // 4. Allocate the transaction price to the performance obligations
        // 5. Recognize revenue when (or as) the entity satisfies a performance obligation

        // Step 1: Contract exists
        if (!revenueContext.HasContract)
        {
            var details = new
            {
                TransactionId = revenueContext.TransactionId,
                RevenueAmount = revenueContext.RevenueAmount,
                Issue = "No contract identified"
            };

            violations.Add(CreateViolation(
                "Transaction",
                revenueContext.TransactionId,
                "Revenue recognized without an identified contract with the customer (ASC 606 Step 1).",
                ViolationSeverity.Critical,
                revenueContext.RevenueAmount,
                JsonSerializer.Serialize(details)
            ));
        }

        // Step 2: Performance obligations identified
        if (!revenueContext.PerformanceObligationsIdentified)
        {
            var details = new
            {
                TransactionId = revenueContext.TransactionId,
                RevenueAmount = revenueContext.RevenueAmount,
                Issue = "Performance obligations not identified"
            };

            violations.Add(CreateViolation(
                "Transaction",
                revenueContext.TransactionId,
                "Performance obligations not properly identified in the contract (ASC 606 Step 2).",
                ViolationSeverity.High,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        // Step 3: Transaction price determined
        if (revenueContext.TransactionPrice <= 0)
        {
            var details = new
            {
                TransactionId = revenueContext.TransactionId,
                TransactionPrice = revenueContext.TransactionPrice,
                Issue = "Transaction price not properly determined"
            };

            violations.Add(CreateViolation(
                "Transaction",
                revenueContext.TransactionId,
                "Transaction price not properly determined (ASC 606 Step 3).",
                ViolationSeverity.High,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        // Step 5: Revenue recognition timing
        if (revenueContext.RevenueRecognized && !revenueContext.PerformanceObligationsSatisfied)
        {
            var details = new
            {
                TransactionId = revenueContext.TransactionId,
                RevenueAmount = revenueContext.RevenueAmount,
                RecognitionDate = revenueContext.RecognitionDate,
                Issue = "Revenue recognized before performance obligations satisfied"
            };

            violations.Add(CreateViolation(
                "Transaction",
                revenueContext.TransactionId,
                $"Revenue of ${revenueContext.RevenueAmount:N2} recognized before performance obligations were satisfied (ASC 606 Step 5).",
                ViolationSeverity.Critical,
                revenueContext.RevenueAmount,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Transaction {revenueContext.TransactionId} complies with ASC 606 revenue recognition")
            : ComplianceResult.NonCompliant($"Transaction {revenueContext.TransactionId} has {violations.Count} revenue recognition violations", violations);
    }

    /// <summary>
    /// Context for revenue recognition evaluation
    /// </summary>
    public class RevenueRecognitionContext
    {
        public Guid TransactionId { get; set; }
        public bool HasContract { get; set; }
        public bool PerformanceObligationsIdentified { get; set; }
        public decimal TransactionPrice { get; set; }
        public bool RevenueRecognized { get; set; }
        public bool PerformanceObligationsSatisfied { get; set; }
        public decimal RevenueAmount { get; set; }
        public DateTime RecognitionDate { get; set; }
    }
}
