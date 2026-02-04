/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

// Currency formatting
export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0
  }).format(amount);
}

export function formatCurrencyDetailed(amount: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  }).format(amount);
}

// Date formatting
export function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric'
  });
}

// Percentage formatting
export function formatPercentage(value: number, decimals: number = 1): string {
  return `${value.toFixed(decimals)}%`;
}

// Calculate change percentage
export function calculateChangePercentage(current: number, previous: number): string {
  if (previous === 0) return '+0.0%';
  const change = ((current - previous) / previous) * 100;
  const sign = change >= 0 ? '+' : '';
  return `${sign}${change.toFixed(1)}%`;
}

// Journal Entry validation
export function validateJournalEntry(debits: number, credits: number): boolean {
  return Math.abs(debits - credits) < 0.01; // Allow for floating point rounding
}

// Calculate days overdue
export function daysOverdue(dueDate: string): number {
  const due = new Date(dueDate);
  const today = new Date();
  const diff = today.getTime() - due.getTime();
  return Math.floor(diff / (1000 * 60 * 60 * 24));
}

// Determine bill/invoice status based on dates
export function determineStatus(dueDate: string, paidAmount: number, totalAmount: number): string {
  if (paidAmount >= totalAmount) return 'Paid';
  if (paidAmount > 0) return 'Partial';
  
  const days = daysOverdue(dueDate);
  if (days > 0) return 'Overdue';
  if (days > -7) return 'Due Soon';
  
  return 'Open';
}

// Calculate 280E tax impact
export function calculate280ETaxImpact(
  deductibleCOGS: number,
  nonDeductibleExpenses: number,
  revenue: number,
  standardTaxRate: number = 0.21
): {
  taxableIncome: number;
  effectiveTaxRate: number;
  additionalTaxBurden: number;
  standardTax: number;
  actualTax: number;
} {
  const grossProfit = revenue - deductibleCOGS;
  const standardTaxableIncome = grossProfit - nonDeductibleExpenses;
  const actualTaxableIncome = grossProfit; // Can't deduct expenses under 280E
  
  const standardTax = standardTaxableIncome * standardTaxRate;
  const actualTax = actualTaxableIncome * standardTaxRate;
  const additionalTaxBurden = actualTax - standardTax;
  
  const effectiveTaxRate = revenue > 0 ? (actualTax / revenue) * 100 : 0;
  
  return {
    taxableIncome: actualTaxableIncome,
    effectiveTaxRate,
    additionalTaxBurden,
    standardTax,
    actualTax
  };
}

// Export data to CSV
export function exportToCSV(data: any[], filename: string): void {
  if (data.length === 0) return;
  
  const headers = Object.keys(data[0]);
  const csv = [
    headers.join(','),
    ...data.map(row => headers.map(header => {
      const value = row[header];
      // Escape values that contain commas or quotes
      if (typeof value === 'string' && (value.includes(',') || value.includes('"'))) {
        return `"${value.replace(/"/g, '""')}"`;
      }
      return value;
    }).join(','))
  ].join('\n');
  
  const blob = new Blob([csv], { type: 'text/csv' });
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = filename;
  a.click();
  window.URL.revokeObjectURL(url);
}

// Get status badge color
export function getStatusColor(status: string): string {
  switch (status.toLowerCase()) {
    case 'posted':
    case 'paid':
      return '#10b981'; // Green
    case 'draft':
    case 'open':
      return '#f59e0b'; // Orange
    case 'overdue':
    case 'void':
      return '#ef4444'; // Red
    case 'partial':
      return '#3b82f6'; // Blue
    default:
      return '#64748b'; // Gray
  }
}

// Calculate running balance
export function calculateRunningBalance(
  transactions: Array<{ debit: number; credit: number }>,
  startingBalance: number = 0
): number[] {
  let balance = startingBalance;
  return transactions.map(txn => {
    balance += txn.debit - txn.credit;
    return balance;
  });
}

// Generate account code
export function generateAccountCode(type: string, lastCode?: string): string {
  const ranges: { [key: string]: [number, number] } = {
    'Asset': [1000, 1999],
    'Liability': [2000, 2999],
    'Equity': [3000, 3999],
    'Revenue': [4000, 4999],
    'COGS': [5000, 5099],
    'Expense': [5100, 5999]
  };
  
  const [min, max] = ranges[type] || [1000, 1999];
  
  if (lastCode) {
    const nextCode = parseInt(lastCode) + 10;
    return nextCode <= max ? nextCode.toString() : min.toString();
  }
  
  return min.toString();
}

// Aggregate accounts by type
export function aggregateAccountsByType(accounts: Array<{ type: string; balance: number }>) {
  const aggregated: { [key: string]: number } = {};
  
  accounts.forEach(account => {
    if (!aggregated[account.type]) {
      aggregated[account.type] = 0;
    }
    aggregated[account.type] += account.balance;
  });
  
  return aggregated;
}

// Calculate financial ratios
export function calculateFinancialRatios(
  currentAssets: number,
  currentLiabilities: number,
  totalAssets: number,
  totalLiabilities: number,
  revenue: number,
  netIncome: number
) {
  return {
    currentRatio: currentLiabilities > 0 ? currentAssets / currentLiabilities : 0,
    debtToAssetRatio: totalAssets > 0 ? totalLiabilities / totalAssets : 0,
    profitMargin: revenue > 0 ? (netIncome / revenue) * 100 : 0,
    returnOnAssets: totalAssets > 0 ? (netIncome / totalAssets) * 100 : 0
  };
}
