/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

export type AccountType = 'Asset' | 'Liability' | 'Equity' | 'Revenue' | 'COGS' | 'Expense';

export type JournalEntryStatus = 'Draft' | 'Posted' | 'Void';

export type BillStatus = 'Draft' | 'Open' | 'Paid' | 'Overdue' | 'Partial';

export type InvoiceStatus = 'Draft' | 'Sent' | 'Paid' | 'Overdue' | 'Partial';

export type AccountingStandard = 'US GAAP' | 'IFRS' | 'Dual Reporting';

export interface Account {
  id: string;
  code: string;
  name: string;
  type: AccountType;
  balance: number;
  parent?: string;
  is280EDeductible?: boolean;
  cannabisRelated?: boolean;
  icon?: string;
  description?: string;
}

export interface JournalEntryLine {
  id: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  description: string;
  debit: number;
  credit: number;
}

export interface JournalEntry {
  id: string;
  date: string;
  description: string;
  status: JournalEntryStatus;
  lines: JournalEntryLine[];
  createdBy: string;
  createdAt: string;
  postedAt?: string;
}

export interface GLTransaction {
  id: string;
  date: string;
  jeNumber: string;
  accountId: string;
  accountCode: string;
  accountName: string;
  description: string;
  debit: number;
  credit: number;
  balance: number;
  source: string;
}

export interface Vendor {
  id: string;
  name: string;
  email: string;
  phone: string;
  address: string;
  paymentTerms: string;
  balance: number;
  totalPaid: number;
  lastPayment?: string;
  w9OnFile: boolean;
  coiOnFile: boolean;
  notes?: string;
}

export interface Customer {
  id: string;
  name: string;
  email: string;
  phone: string;
  address: string;
  paymentTerms: string;
  creditLimit: number;
  balance: number;
  totalRevenue: number;
  lastInvoice?: string;
  cannabisLicense?: string;
  licenseExpiry?: string;
  notes?: string;
}

export interface Bill {
  id: string;
  vendorId: string;
  vendorName: string;
  date: string;
  dueDate: string;
  amount: number;
  paidAmount: number;
  status: BillStatus;
  description: string;
  invoiceNumber?: string;
  glAccount?: string;
}

export interface Invoice {
  id: string;
  customerId: string;
  customerName: string;
  date: string;
  dueDate: string;
  amount: number;
  paidAmount: number;
  status: InvoiceStatus;
  description: string;
  invoiceNumber: string;
  items: InvoiceItem[];
}

export interface InvoiceItem {
  id: string;
  description: string;
  quantity: number;
  rate: number;
  amount: number;
  glAccount?: string;
}

export interface Payment {
  id: string;
  date: string;
  amount: number;
  paymentMethod: string;
  referenceNumber?: string;
  notes?: string;
}

export interface MetricData {
  title: string;
  value: string | number;
  change?: string;
  icon: string;
  color: string;
}

export interface ChartData {
  date?: string;
  name?: string;
  value?: number;
  [key: string]: any;
}

export interface FinanceSettings {
  accountingStandard: AccountingStandard;
  fiscalYearStart: string;
  currency: string;
  timezone: string;
  defaultPaymentTerms: string;
}

export interface Tax280EData {
  deductibleCOGS: number;
  nonDeductibleExpenses: number;
  effectiveTaxRate: number;
  standardTaxRate: number;
  additionalTaxBurden: number;
  totalRevenue: number;
  grossProfit: number;
  netIncome: number;
}

export interface AgingReportLine {
  name: string;
  current: number;
  days1_30: number;
  days31_60: number;
  days61_90: number;
  days90Plus: number;
  total: number;
}
