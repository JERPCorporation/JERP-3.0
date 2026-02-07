/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

// Enums
export enum AccountType {
  Asset = 'Asset',
  Liability = 'Liability',
  Equity = 'Equity',
  Revenue = 'Revenue',
  Expense = 'Expense',
}

export enum JournalEntryStatus {
  Draft = 'Draft',
  Posted = 'Posted',
  Voided = 'Voided',
}

export enum BillStatus {
  Draft = 'Draft',
  Approved = 'Approved',
  Paid = 'Paid',
  PartiallyPaid = 'PartiallyPaid',
  Overdue = 'Overdue',
}

export enum InvoiceStatus {
  Draft = 'Draft',
  Sent = 'Sent',
  Paid = 'Paid',
  PartiallyPaid = 'PartiallyPaid',
  Overdue = 'Overdue',
}

// Chart of Accounts
export interface Account {
  id: string;
  companyId: string;
  accountNumber: string;
  accountName: string;
  type: AccountType;
  balance: number;
  isActive: boolean;
  isSystemAccount: boolean;
  isCOGS: boolean;
  isNonDeductible: boolean;
  taxCategory?: string;
  fasbTopicId?: string;
  fasbSubtopicId?: string;
  fasbReference?: string;
  fasbTopicName?: string;
  fasbSubtopicName?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateAccountRequest {
  companyId: string;
  accountNumber: string;
  accountName: string;
  type: AccountType;
  isCOGS?: boolean;
  isNonDeductible?: boolean;
  taxCategory?: string;
  fasbTopicId?: string;
  fasbSubtopicId?: string;
  fasbReference?: string;
}

export interface UpdateAccountRequest {
  accountName: string;
  isActive: boolean;
  isCOGS?: boolean;
  isNonDeductible?: boolean;
  taxCategory?: string;
  fasbTopicId?: string;
  fasbSubtopicId?: string;
  fasbReference?: string;
}

// Journal Entries
export interface JournalEntry {
  id: string;
  companyId: string;
  journalEntryNumber: string;
  transactionDate: string;
  description: string;
  status: JournalEntryStatus;
  source: string;
  totalDebit: number;
  totalCredit: number;
  isBalanced: boolean;
  postedAt?: string;
  createdAt: string;
  updatedAt: string;
  ledgerEntries?: GeneralLedgerEntry[];
}

export interface GeneralLedgerEntry {
  id: string;
  companyId: string;
  journalEntryId: string;
  accountId: string;
  accountNumber: string;
  accountName: string;
  transactionDate: string;
  debitAmount: number;
  creditAmount: number;
  description: string;
  source: string;
  sourceEntityId?: string;
  is280EDeductible: boolean;
  createdAt: string;
}

export interface CreateJournalEntryRequest {
  companyId: string;
  transactionDate: string;
  description: string;
  entries: {
    accountId: string;
    debitAmount: number;
    creditAmount: number;
    description: string;
  }[];
}

// Vendors (AP)
export interface Vendor {
  id: string;
  companyId: string;
  vendorNumber: string;
  companyName: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  address?: string;
  balance: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateVendorDto {
  companyName: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  address?: string;
}

export interface UpdateVendorDto {
  companyName?: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  address?: string;
  isActive?: boolean;
}

// Bills (AP)
export interface VendorBill {
  id: string;
  companyId: string;
  billNumber: string;
  vendorId: string;
  vendorName: string;
  billDate: string;
  dueDate: string;
  totalAmount: number;
  amountDue: number;
  status: BillStatus;
  createdAt: string;
  updatedAt: string;
}

// Customers (AR)
export interface Customer {
  id: string;
  companyId: string;
  customerNumber: string;
  companyName: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  balance: number;
  creditLimit: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

// Invoices (AR)
export interface Invoice {
  id: string;
  companyId: string;
  invoiceNumber: string;
  customerId: string;
  customerName: string;
  invoiceDate: string;
  dueDate: string;
  totalAmount: number;
  amountDue: number;
  status: InvoiceStatus;
  createdAt: string;
  updatedAt: string;
}

// Financial Reports
export interface ReportRequest {
  companyId: string;
  startDate: string;
  endDate: string;
}

export interface ProfitAndLossReport {
  companyId: string;
  startDate: string;
  endDate: string;
  revenue: ReportLineItem[];
  totalRevenue: number;
  expenses: ReportLineItem[];
  totalExpenses: number;
  netIncome: number;
  profitMargin: number;
}

export interface BalanceSheetReport {
  companyId: string;
  asOfDate: string;
  assets: {
    current: ReportLineItem[];
    totalCurrent: number;
    fixed: ReportLineItem[];
    totalFixed: number;
    totalAssets: number;
  };
  liabilities: {
    current: ReportLineItem[];
    totalCurrent: number;
    longTerm: ReportLineItem[];
    totalLongTerm: number;
    totalLiabilities: number;
  };
  equity: {
    items: ReportLineItem[];
    totalEquity: number;
  };
  balanced: boolean;
}

export interface ReportLineItem {
  account: string;
  amount: number;
  percentage?: number;
}

// API Response wrapper
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

// Pagination
export interface PaginatedResponse<T> {
  data: T[];
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
}
