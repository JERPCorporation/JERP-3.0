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
/// GAAP Balance Sheet rule: Assets = Liabilities + Equity
/// </summary>
public class GAAPBalanceSheetRule : ComplianceRuleBase
{
    public override string RuleName => "GAAP Balance Sheet Equation";
    public override ViolationType ViolationType => ViolationType.FinancialCompliance;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Critical;

    private const decimal CriticalThreshold = 1000.00m;
    private const decimal HighThreshold = 100.00m;

    public GAAPBalanceSheetRule(ILogger<GAAPBalanceSheetRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not BalanceSheetContext balanceSheet)
        {
            throw new ArgumentException("Context must be a BalanceSheetContext", nameof(context));
        }

        LogEvaluationStart("BalanceSheet", balanceSheet.CompanyId);

        var violations = new List<ComplianceViolation>();

        // Calculate balance sheet equation: Assets = Liabilities + Equity
        var leftSide = balanceSheet.TotalAssets;
        var rightSide = balanceSheet.TotalLiabilities + balanceSheet.TotalEquity;
        var imbalance = Math.Abs(leftSide - rightSide);

        if (imbalance > 0.01m) // Allow for minor rounding differences
        {
            var severity = imbalance switch
            {
                > CriticalThreshold => ViolationSeverity.Critical,
                > HighThreshold => ViolationSeverity.High,
                _ => ViolationSeverity.Medium
            };

            var details = new
            {
                TotalAssets = balanceSheet.TotalAssets,
                TotalLiabilities = balanceSheet.TotalLiabilities,
                TotalEquity = balanceSheet.TotalEquity,
                Imbalance = imbalance,
                AsOfDate = balanceSheet.AsOfDate
            };

            violations.Add(CreateViolation(
                "Company",
                balanceSheet.CompanyId,
                $"Balance sheet equation does not balance. Assets: ${leftSide:N2}, Liabilities + Equity: ${rightSide:N2}, Imbalance: ${imbalance:N2}",
                severity,
                imbalance,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Balance sheet for company {balanceSheet.CompanyId} is balanced")
            : ComplianceResult.NonCompliant($"Balance sheet for company {balanceSheet.CompanyId} has {violations.Count} violations", violations);
    }

    /// <summary>
    /// Context for balance sheet evaluation
    /// </summary>
    public class BalanceSheetContext
    {
        public Guid CompanyId { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalEquity { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
