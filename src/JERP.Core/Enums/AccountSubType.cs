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

namespace JERP.Core.Enums;

/// <summary>
/// Represents the sub-type classification for accounts
/// </summary>
public enum AccountSubType
{
    // Asset sub-types
    Cash,
    BankAccount,
    AccountsReceivable,
    Inventory,
    PrepaidExpense,
    FixedAsset,
    OtherAsset,

    // Liability sub-types
    AccountsPayable,
    CreditCard,
    TaxPayable,
    PayrollLiability,
    LongTermDebt,
    OtherLiability,

    // Equity sub-types
    OwnersEquity,
    RetainedEarnings,
    Distributions,

    // Revenue sub-types
    ProductSales,
    ServiceRevenue,
    OtherIncome,

    // Expense sub-types
    COGS,
    PayrollExpense,
    PayrollTaxExpense,
    RentExpense,
    UtilitiesExpense,
    MarketingExpense,
    OfficeExpense,
    InsuranceExpense,
    ProfessionalFees,
    OtherExpense
}
