/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import { mockAccounts, mockJournalEntries, mockVendors, mockCustomers, mockBills, mockInvoices } from './mock-data';

export const financeAPI = {
  // Chart of Accounts operations
  fetchAccountLedger: async (filters?: { searchQuery?: string; accountCategory?: string }) => {
    await new Promise(resolve => setTimeout(resolve, 100)); // Simulate network delay
    let results = [...mockAccounts];
    
    if (filters?.searchQuery) {
      const query = filters.searchQuery.toLowerCase();
      results = results.filter(acc => 
        acc.name.toLowerCase().includes(query) || acc.code.includes(query)
      );
    }
    
    if (filters?.accountCategory && filters.accountCategory !== 'All') {
      results = results.filter(acc => acc.type === filters.accountCategory);
    }
    
    return { data: results, success: true };
  },
  
  addNewAccount: async (accountData: any) => {
    await new Promise(resolve => setTimeout(resolve, 200));
    return { success: true, message: 'Account created successfully' };
  },
  
  // Journal Entries operations
  fetchTransactionLog: async (dateRange?: { from: string; to: string }) => {
    await new Promise(resolve => setTimeout(resolve, 100));
    return { data: mockJournalEntries, success: true };
  },
  
  submitNewEntry: async (entryData: any) => {
    await new Promise(resolve => setTimeout(resolve, 200));
    return { success: true, message: 'Journal entry posted successfully' };
  },
  
  // Vendor (AP) operations
  fetchVendorRegistry: async () => {
    await new Promise(resolve => setTimeout(resolve, 100));
    return { data: mockVendors, success: true };
  },
  
  fetchOutstandingBills: async (statusFilter?: string) => {
    await new Promise(resolve => setTimeout(resolve, 100));
    let results = [...mockBills];
    
    if (statusFilter && statusFilter !== 'All') {
      results = results.filter(bill => bill.status === statusFilter);
    }
    
    return { data: results, success: true };
  },
  
  // Customer (AR) operations
  fetchCustomerDirectory: async () => {
    await new Promise(resolve => setTimeout(resolve, 100));
    return { data: mockCustomers, success: true };
  },
  
  fetchOpenInvoices: async (statusFilter?: string) => {
    await new Promise(resolve => setTimeout(resolve, 100));
    let results = [...mockInvoices];
    
    if (statusFilter && statusFilter !== 'All') {
      results = results.filter(inv => inv.status === statusFilter);
    }
    
    return { data: results, success: true };
  },
  
  // Reporting operations
  generateAgingSummary: async (reportType: 'AccountsPayable' | 'AccountsReceivable') => {
    await new Promise(resolve => setTimeout(resolve, 150));
    
    if (reportType === 'AccountsPayable') {
      return {
        data: [
          { entity: 'Green Leaf Supply Co.', notDue: 12340, period1: 0, period2: 0, period3: 0, period4: 0, totalBalance: 12340 },
          { entity: 'Pacific Packaging Inc.', notDue: 8450, period1: 0, period2: 0, period3: 0, period4: 0, totalBalance: 8450 },
          { entity: 'CannaTech Labs', notDue: 0, period1: 3200, period2: 0, period3: 0, period4: 0, totalBalance: 3200 },
        ],
        totals: { notDue: 20790, period1: 3200, period2: 0, period3: 0, period4: 0, totalBalance: 23990 },
        success: true
      };
    } else {
      return {
        data: [
          { entity: 'Green Valley Dispensary', notDue: 22450, period1: 12450, period2: 0, period3: 0, period4: 0, totalBalance: 34900 },
          { entity: 'Emerald Coast Cannabis', notDue: 0, period1: 8750, period2: 0, period3: 0, period4: 0, totalBalance: 8750 },
          { entity: 'Bay Area Wellness Center', notDue: 15600, period1: 0, period2: 0, period3: 0, period4: 0, totalBalance: 15600 },
        ],
        totals: { notDue: 38050, period1: 21200, period2: 0, period3: 0, period4: 0, totalBalance: 59250 },
        success: true
      };
    }
  },
  
  generateProfitLossStatement: async (startPeriod: string, endPeriod: string) => {
    await new Promise(resolve => setTimeout(resolve, 150));
    return {
      data: {
        revenueLines: [
          { accountName: 'Cannabis Sales - Flower', amount: 320000 },
          { accountName: 'Cannabis Sales - Edibles', amount: 180000 },
        ],
        totalRevenue: 500000,
        cogsLines: [
          { accountName: 'COGS - Cannabis Product', amount: 120000 },
        ],
        totalCOGS: 120000,
        grossProfit: 380000,
        grossMargin: 76,
        expenseLines: [
          { accountName: 'Payroll Expense', amount: 85000, percentOfRevenue: 17 },
          { accountName: 'Rent Expense', amount: 15000, percentOfRevenue: 3 },
          { accountName: 'Marketing & Advertising', amount: 12000, percentOfRevenue: 2.4 },
          { accountName: 'Utilities', amount: 8000, percentOfRevenue: 1.6 },
        ],
        totalExpenses: 120000,
        netIncome: 260000,
        netMargin: 52
      },
      success: true
    };
  },
  
  generateBalanceSheet: async (asOfDate: string) => {
    await new Promise(resolve => setTimeout(resolve, 150));
    return {
      data: {
        assets: {
          currentAssets: [
            { accountName: 'Cash - Operating', amount: 450000 },
            { accountName: 'Cash - Vault', amount: 125000 },
            { accountName: 'Accounts Receivable', amount: 85000 },
            { accountName: 'Inventory', amount: 150000 },
          ],
          totalCurrentAssets: 810000,
          fixedAssets: [
            { accountName: 'Equipment', amount: 150000 },
          ],
          totalFixedAssets: 150000,
          totalAssets: 960000
        },
        liabilities: {
          currentLiabilities: [
            { accountName: 'Accounts Payable', amount: 45000 },
            { accountName: 'Payroll Tax Liability', amount: 12500 },
          ],
          totalCurrentLiabilities: 57500,
          longTermLiabilities: [
            { accountName: 'Business Loan', amount: 200000 },
          ],
          totalLongTermLiabilities: 200000,
          totalLiabilities: 257500
        },
        equity: {
          equityAccounts: [
            { accountName: "Owner's Equity", amount: 500000 },
            { accountName: 'Retained Earnings', amount: 202500 },
          ],
          totalEquity: 702500
        },
        totalLiabilitiesAndEquity: 960000,
        isBalanced: true
      },
      success: true
    };
  }
};
