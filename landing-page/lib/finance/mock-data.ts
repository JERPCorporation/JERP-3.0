/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import { Account, JournalEntry, GLTransaction, Vendor, Customer, Bill, Invoice, InvoiceItem } from './types';

// Chart of Accounts
export const mockAccounts: Account[] = [
  // ASSETS (1000-1999)
  { id: '1000', code: '1000', name: 'Cash - Operating', type: 'Asset', balance: 158432, description: 'Primary operating cash account', fasbTopicId: '305', fasbSubtopicId: '10' },
  { id: '1010', code: '1010', name: 'Cash - Vault (Cannabis)', type: 'Asset', balance: 45200, cannabisRelated: true, icon: '🌿', description: 'Secure cash vault for cannabis operations', fasbTopicId: '305', fasbSubtopicId: '10' },
  { id: '1020', code: '1020', name: 'Bank - Checking', type: 'Asset', balance: 225840, description: 'Primary checking account', fasbTopicId: '305', fasbSubtopicId: '10' },
  { id: '1030', code: '1030', name: 'Bank - Savings', type: 'Asset', balance: 50000, description: 'Business savings account', fasbTopicId: '305', fasbSubtopicId: '10' },
  { id: '1200', code: '1200', name: 'Accounts Receivable', type: 'Asset', balance: 128450, description: 'Customer receivables', fasbTopicId: '310', fasbSubtopicId: '10' },
  { id: '1300', code: '1300', name: 'Inventory - Cannabis Flower', type: 'Asset', balance: 85600, cannabisRelated: true, icon: '🌿', description: 'Cannabis flower inventory', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '1310', code: '1310', name: 'Inventory - Edibles', type: 'Asset', balance: 42300, cannabisRelated: true, icon: '🍬', description: 'Cannabis edibles inventory', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '1320', code: '1320', name: 'Inventory - Concentrates', type: 'Asset', balance: 28900, cannabisRelated: true, icon: '💎', description: 'Cannabis concentrates inventory', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '1500', code: '1500', name: 'Equipment - Cultivation', type: 'Asset', balance: 125000, description: 'Cultivation equipment', fasbTopicId: '360', fasbSubtopicId: '10' },
  { id: '1510', code: '1510', name: 'Equipment - Processing', type: 'Asset', balance: 85000, description: 'Processing equipment', fasbTopicId: '360', fasbSubtopicId: '10' },
  { id: '1600', code: '1600', name: 'Accumulated Depreciation', type: 'Asset', balance: -32000, description: 'Equipment depreciation', fasbTopicId: '360', fasbSubtopicId: '10' },
  
  // LIABILITIES (2000-2999)
  { id: '2000', code: '2000', name: 'Accounts Payable', type: 'Liability', balance: 45230, description: 'Vendor payables', fasbTopicId: '405', fasbSubtopicId: '10' },
  { id: '2100', code: '2100', name: 'Payroll Tax Liability', type: 'Liability', balance: 12340, description: 'Payroll tax obligations', fasbTopicId: '405', fasbSubtopicId: '10' },
  { id: '2110', code: '2110', name: 'Sales Tax Liability', type: 'Liability', balance: 18750, description: 'Sales tax obligations', fasbTopicId: '405', fasbSubtopicId: '10' },
  { id: '2120', code: '2120', name: 'Excise Tax Liability', type: 'Liability', balance: 15600, cannabisRelated: true, icon: '🌿', description: 'Cannabis excise tax', fasbTopicId: '405', fasbSubtopicId: '10' },
  { id: '2200', code: '2200', name: 'Credit Card Payable', type: 'Liability', balance: 8450, description: 'Credit card balances', fasbTopicId: '405', fasbSubtopicId: '10' },
  { id: '2300', code: '2300', name: 'Long-term Debt', type: 'Liability', balance: 68000, description: 'Business loans', fasbTopicId: '405', fasbSubtopicId: '10' },
  
  // EQUITY (3000-3999)
  { id: '3000', code: '3000', name: 'Common Stock', type: 'Equity', balance: 500000, description: 'Owner equity', fasbTopicId: '505', fasbSubtopicId: '10' },
  { id: '3100', code: '3100', name: 'Retained Earnings', type: 'Equity', balance: 150552, description: 'Accumulated profits', fasbTopicId: '505', fasbSubtopicId: '10' },
  { id: '3200', code: '3200', name: 'Owners Draw', type: 'Equity', balance: -25000, description: 'Owner distributions', fasbTopicId: '505', fasbSubtopicId: '10' },
  
  // REVENUE (4000-4999)
  { id: '4000', code: '4000', name: 'Cannabis Sales - Flower', type: 'Revenue', balance: 145230, cannabisRelated: true, icon: '🌿', description: 'Flower product sales', fasbTopicId: '605', fasbSubtopicId: '10' },
  { id: '4010', code: '4010', name: 'Cannabis Sales - Edibles', type: 'Revenue', balance: 68450, cannabisRelated: true, icon: '🍬', description: 'Edibles product sales', fasbTopicId: '605', fasbSubtopicId: '10' },
  { id: '4020', code: '4020', name: 'Cannabis Sales - Concentrates', type: 'Revenue', balance: 32210, cannabisRelated: true, icon: '💎', description: 'Concentrates product sales', fasbTopicId: '605', fasbSubtopicId: '10' },
  { id: '4100', code: '4100', name: 'Wholesale Revenue', type: 'Revenue', balance: 58900, cannabisRelated: true, description: 'B2B wholesale sales', fasbTopicId: '605', fasbSubtopicId: '10' },
  { id: '4200', code: '4200', name: 'Consulting Revenue', type: 'Revenue', balance: 12400, description: 'Consulting services', fasbTopicId: '605', fasbSubtopicId: '10' },
  
  // COGS (5000-5099) - 280E Deductible
  { id: '5000', code: '5000', name: 'COGS - Cannabis Product', type: 'COGS', balance: 45600, is280EDeductible: true, icon: '✅', description: 'Direct product costs', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '5010', code: '5010', name: 'COGS - Cultivation Labor', type: 'COGS', balance: 22300, is280EDeductible: true, icon: '✅', description: 'Growing labor costs', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '5020', code: '5020', name: 'COGS - Packaging', type: 'COGS', balance: 8900, is280EDeductible: true, icon: '✅', description: 'Product packaging', fasbTopicId: '330', fasbSubtopicId: '10' },
  { id: '5030', code: '5030', name: 'COGS - Testing & Compliance', type: 'COGS', balance: 6800, is280EDeductible: true, icon: '✅', description: 'Lab testing costs', fasbTopicId: '330', fasbSubtopicId: '10' },
  
  // EXPENSES (5100-5999) - 280E Non-Deductible
  { id: '5100', code: '5100', name: 'Payroll Expense - Budtenders', type: 'Expense', balance: 28450, is280EDeductible: false, icon: '❌', description: 'Retail staff wages', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5110', code: '5110', name: 'Payroll Expense - Management', type: 'Expense', balance: 18200, is280EDeductible: false, icon: '❌', description: 'Management salaries', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5120', code: '5120', name: 'Payroll Tax Expense', type: 'Expense', balance: 6240, is280EDeductible: false, icon: '❌', description: 'Employer payroll taxes', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5200', code: '5200', name: 'Rent Expense', type: 'Expense', balance: 15000, is280EDeductible: false, icon: '❌', description: 'Facility rent', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5210', code: '5210', name: 'Utilities', type: 'Expense', balance: 4500, is280EDeductible: false, icon: '❌', description: 'Electricity, water, gas', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5300', code: '5300', name: 'Marketing & Advertising', type: 'Expense', balance: 8900, is280EDeductible: false, icon: '❌', description: 'Marketing costs', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5310', code: '5310', name: 'Professional Fees', type: 'Expense', balance: 12400, is280EDeductible: false, icon: '❌', description: 'Legal, accounting fees', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5400', code: '5400', name: 'Insurance', type: 'Expense', balance: 6800, is280EDeductible: false, icon: '❌', description: 'Business insurance', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5410', code: '5410', name: 'Office Supplies', type: 'Expense', balance: 1240, is280EDeductible: false, icon: '❌', description: 'Office materials', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5500', code: '5500', name: 'Depreciation Expense', type: 'Expense', balance: 2800, is280EDeductible: false, icon: '❌', description: 'Equipment depreciation', fasbTopicId: '720', fasbSubtopicId: '10' },
  { id: '5600', code: '5600', name: 'Interest Expense', type: 'Expense', balance: 1200, is280EDeductible: false, icon: '❌', description: 'Loan interest', fasbTopicId: '720', fasbSubtopicId: '10' },
];

// Journal Entries
export const mockJournalEntries: JournalEntry[] = [
  {
    id: 'JE-001',
    date: '2025-02-04',
    description: 'Daily Cannabis Sales - Flower',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-04T10:30:00Z',
    postedAt: '2025-02-04T10:35:00Z',
    lines: [
      { id: 'L1', accountId: '1000', accountCode: '1000', accountName: 'Cash - Operating', description: 'Daily sales', debit: 12450, credit: 0 },
      { id: 'L2', accountId: '4000', accountCode: '4000', accountName: 'Cannabis Sales - Flower', description: 'Flower sales', debit: 0, credit: 12450 }
    ]
  },
  {
    id: 'JE-002',
    date: '2025-02-04',
    description: 'Payroll - Feb 2025',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-04T09:00:00Z',
    postedAt: '2025-02-04T09:15:00Z',
    lines: [
      { id: 'L3', accountId: '5100', accountCode: '5100', accountName: 'Payroll Expense - Budtenders', description: 'Biweekly payroll', debit: 8450, credit: 0 },
      { id: 'L4', accountId: '5120', accountCode: '5120', accountName: 'Payroll Tax Expense', description: 'Employer taxes', debit: 1240, credit: 0 },
      { id: 'L5', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Payroll disbursement', debit: 0, credit: 9690 }
    ]
  },
  {
    id: 'JE-003',
    date: '2025-02-03',
    description: 'Vendor Payment - Green Leaf Supply',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-03T14:20:00Z',
    postedAt: '2025-02-03T14:25:00Z',
    lines: [
      { id: 'L6', accountId: '2000', accountCode: '2000', accountName: 'Accounts Payable', description: 'Invoice payment', debit: 8450, credit: 0 },
      { id: 'L7', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Payment to vendor', debit: 0, credit: 8450 }
    ]
  },
  {
    id: 'JE-004',
    date: '2025-02-03',
    description: 'Rent Payment - February',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-03T11:00:00Z',
    postedAt: '2025-02-03T11:05:00Z',
    lines: [
      { id: 'L8', accountId: '5200', accountCode: '5200', accountName: 'Rent Expense', description: 'Monthly rent', debit: 5000, credit: 0 },
      { id: 'L9', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Rent payment', debit: 0, credit: 5000 }
    ]
  },
  {
    id: 'JE-005',
    date: '2025-02-02',
    description: 'Customer Payment - Green Valley Dispensary',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-02T15:30:00Z',
    postedAt: '2025-02-02T15:35:00Z',
    lines: [
      { id: 'L10', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Invoice payment received', debit: 12450, credit: 0 },
      { id: 'L11', accountId: '1200', accountCode: '1200', accountName: 'Accounts Receivable', description: 'Payment applied', debit: 0, credit: 12450 }
    ]
  },
  {
    id: 'JE-006',
    date: '2025-02-01',
    description: 'Inventory Purchase - Cannabis Products',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-02-01T10:00:00Z',
    postedAt: '2025-02-01T10:10:00Z',
    lines: [
      { id: 'L12', accountId: '1300', accountCode: '1300', accountName: 'Inventory - Cannabis Flower', description: 'Inventory purchase', debit: 15600, credit: 0 },
      { id: 'L13', accountId: '2000', accountCode: '2000', accountName: 'Accounts Payable', description: 'Vendor invoice', debit: 0, credit: 15600 }
    ]
  },
  {
    id: 'JE-007',
    date: '2025-01-31',
    description: 'Utilities - January',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-01-31T16:00:00Z',
    postedAt: '2025-01-31T16:05:00Z',
    lines: [
      { id: 'L14', accountId: '5210', accountCode: '5210', accountName: 'Utilities', description: 'Monthly utilities', debit: 1500, credit: 0 },
      { id: 'L15', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Utility payment', debit: 0, credit: 1500 }
    ]
  },
  {
    id: 'JE-008',
    date: '2025-01-30',
    description: 'Marketing Campaign - Social Media',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-01-30T13:00:00Z',
    postedAt: '2025-01-30T13:10:00Z',
    lines: [
      { id: 'L16', accountId: '5300', accountCode: '5300', accountName: 'Marketing & Advertising', description: 'Social media ads', debit: 2400, credit: 0 },
      { id: 'L17', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Marketing payment', debit: 0, credit: 2400 }
    ]
  },
  {
    id: 'JE-009',
    date: '2025-01-29',
    description: 'Insurance Premium - February',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-01-29T10:00:00Z',
    postedAt: '2025-01-29T10:05:00Z',
    lines: [
      { id: 'L18', accountId: '5400', accountCode: '5400', accountName: 'Insurance', description: 'Monthly insurance', debit: 2300, credit: 0 },
      { id: 'L19', accountId: '1020', accountCode: '1020', accountName: 'Bank - Checking', description: 'Insurance payment', debit: 0, credit: 2300 }
    ]
  },
  {
    id: 'JE-010',
    date: '2025-01-28',
    description: 'Cannabis Wholesale - Green Valley',
    status: 'Posted',
    createdBy: 'admin@company.com',
    createdAt: '2025-01-28T11:30:00Z',
    postedAt: '2025-01-28T11:40:00Z',
    lines: [
      { id: 'L20', accountId: '1200', accountCode: '1200', accountName: 'Accounts Receivable', description: 'Wholesale invoice', debit: 18900, credit: 0 },
      { id: 'L21', accountId: '4100', accountCode: '4100', accountName: 'Wholesale Revenue', description: 'Wholesale sale', debit: 0, credit: 18900 }
    ]
  }
];

// Vendors
export const mockVendors: Vendor[] = [
  {
    id: 'V001',
    name: 'Green Leaf Supply Co.',
    email: 'orders@greenleafsupply.com',
    phone: '(415) 555-0100',
    address: '123 Cannabis Ave, Oakland, CA 94601',
    paymentTerms: 'Net 30',
    balance: 20790,
    totalPaid: 145600,
    lastPayment: '2025-02-03',
    w9OnFile: true,
    coiOnFile: true,
    notes: 'Primary cannabis supplier - excellent quality'
  },
  {
    id: 'V002',
    name: 'Pacific Packaging Inc.',
    email: 'billing@pacificpkg.com',
    phone: '(510) 555-0200',
    address: '456 Industrial Blvd, Hayward, CA 94544',
    paymentTerms: 'Net 15',
    balance: 8450,
    totalPaid: 42300,
    lastPayment: '2025-01-28',
    w9OnFile: true,
    coiOnFile: true,
    notes: 'Child-resistant packaging supplier'
  },
  {
    id: 'V003',
    name: 'CannaTech Labs',
    email: 'testing@cannatechlabs.com',
    phone: '(916) 555-0300',
    address: '789 Lab Drive, Sacramento, CA 95814',
    paymentTerms: 'Due on Receipt',
    balance: 5600,
    totalPaid: 28900,
    lastPayment: '2025-02-01',
    w9OnFile: true,
    coiOnFile: true,
    notes: 'Licensed testing laboratory'
  },
  {
    id: 'V004',
    name: 'Golden State Security',
    email: 'billing@gsstatesec.com',
    phone: '(408) 555-0400',
    address: '321 Security Way, San Jose, CA 95113',
    paymentTerms: 'Net 30',
    balance: 4200,
    totalPaid: 18500,
    lastPayment: '2025-01-15',
    w9OnFile: true,
    coiOnFile: false,
    notes: 'Security services provider'
  },
  {
    id: 'V005',
    name: 'Bay Area Grow Supplies',
    email: 'sales@bayareagrow.com',
    phone: '(650) 555-0500',
    address: '555 Garden Rd, San Mateo, CA 94403',
    paymentTerms: 'Net 30',
    balance: 6190,
    totalPaid: 34200,
    lastPayment: '2025-01-30',
    w9OnFile: true,
    coiOnFile: true,
    notes: 'Cultivation supplies and equipment'
  }
];

// Customers
export const mockCustomers: Customer[] = [
  {
    id: 'C001',
    name: 'Green Valley Dispensary',
    email: 'purchasing@greenvalley.com',
    phone: '(707) 555-1000',
    address: '100 Main St, Napa, CA 94559',
    paymentTerms: 'Net 30',
    creditLimit: 50000,
    balance: 41164,
    totalRevenue: 285400,
    lastInvoice: '2025-01-28',
    cannabisLicense: 'C11-0000001-LIC',
    licenseExpiry: '2026-12-31',
    notes: 'Top customer - wholesale only'
  },
  {
    id: 'C002',
    name: 'Emerald Coast Cannabis',
    email: 'orders@emeraldcoast.com',
    phone: '(831) 555-2000',
    address: '200 Pacific Hwy, Santa Cruz, CA 95060',
    paymentTerms: 'Net 15',
    creditLimit: 35000,
    balance: 28450,
    totalRevenue: 168900,
    lastInvoice: '2025-02-01',
    cannabisLicense: 'C11-0000002-LIC',
    licenseExpiry: '2026-08-15',
    notes: 'Growing account - good payment history'
  },
  {
    id: 'C003',
    name: 'Bay Area Wellness Center',
    email: 'ap@baywellness.com',
    phone: '(415) 555-3000',
    address: '300 Health St, San Francisco, CA 94102',
    paymentTerms: 'Net 30',
    creditLimit: 40000,
    balance: 22100,
    totalRevenue: 142800,
    lastInvoice: '2025-01-25',
    cannabisLicense: 'C11-0000003-LIC',
    licenseExpiry: '2026-06-30',
    notes: 'Medical focus - reliable customer'
  },
  {
    id: 'C004',
    name: 'Central Valley Organics',
    email: 'billing@centralorganics.com',
    phone: '(559) 555-4000',
    address: '400 Farm Rd, Fresno, CA 93721',
    paymentTerms: 'Net 45',
    creditLimit: 25000,
    balance: 18750,
    totalRevenue: 98500,
    lastInvoice: '2025-01-30',
    cannabisLicense: 'C11-0000004-LIC',
    licenseExpiry: '2027-03-15',
    notes: 'Organic products preferred'
  },
  {
    id: 'C005',
    name: 'Coastal Cannabis Collective',
    email: 'orders@coastalccc.com',
    phone: '(805) 555-5000',
    address: '500 Beach Blvd, Ventura, CA 93001',
    paymentTerms: 'Net 30',
    creditLimit: 30000,
    balance: 17986,
    totalRevenue: 124300,
    lastInvoice: '2025-02-02',
    cannabisLicense: 'C11-0000005-LIC',
    licenseExpiry: '2026-11-20',
    notes: 'Premium products preferred'
  }
];

// Bills (Accounts Payable)
export const mockBills: Bill[] = [
  { id: 'AP-145', vendorId: 'V001', vendorName: 'Green Leaf Supply Co.', date: '2025-01-20', dueDate: '2025-02-05', amount: 8450, paidAmount: 0, status: 'Overdue', description: 'Cannabis flower inventory', invoiceNumber: 'INV-2025-0120', glAccount: '1300' },
  { id: 'AP-146', vendorId: 'V002', vendorName: 'Pacific Packaging Inc.', date: '2025-01-25', dueDate: '2025-02-09', amount: 4230, paidAmount: 0, status: 'Overdue', description: 'Child-resistant containers', invoiceNumber: 'PKG-2025-0125', glAccount: '5020' },
  { id: 'AP-147', vendorId: 'V003', vendorName: 'CannaTech Labs', date: '2025-02-01', dueDate: '2025-02-01', amount: 1850, paidAmount: 0, status: 'Overdue', description: 'Product testing services', invoiceNumber: 'TEST-2025-0201', glAccount: '5030' },
  { id: 'AP-148', vendorId: 'V004', vendorName: 'Golden State Security', date: '2025-01-31', dueDate: '2025-02-10', amount: 2100, paidAmount: 0, status: 'Open', description: 'February security services', invoiceNumber: 'SEC-2025-0131', glAccount: '5310' },
  { id: 'AP-149', vendorId: 'V005', vendorName: 'Bay Area Grow Supplies', date: '2025-02-02', dueDate: '2025-02-12', amount: 3560, paidAmount: 0, status: 'Open', description: 'Growing supplies', invoiceNumber: 'GROW-2025-0202', glAccount: '5000' },
  { id: 'AP-150', vendorId: 'V001', vendorName: 'Green Leaf Supply Co.', date: '2025-02-03', dueDate: '2025-02-15', amount: 12340, paidAmount: 0, status: 'Open', description: 'Mixed product order', invoiceNumber: 'INV-2025-0203', glAccount: '1300' },
  { id: 'AP-151', vendorId: 'V002', vendorName: 'Pacific Packaging Inc.', date: '2025-02-04', dueDate: '2025-02-19', amount: 2890, paidAmount: 0, status: 'Open', description: 'Packaging materials', invoiceNumber: 'PKG-2025-0204', glAccount: '5020' },
  { id: 'AP-142', vendorId: 'V001', vendorName: 'Green Leaf Supply Co.', date: '2025-01-10', dueDate: '2025-01-25', amount: 9500, paidAmount: 9500, status: 'Paid', description: 'January inventory', invoiceNumber: 'INV-2025-0110', glAccount: '1300' },
  { id: 'AP-143', vendorId: 'V003', vendorName: 'CannaTech Labs', date: '2025-01-15', dueDate: '2025-01-15', amount: 2200, paidAmount: 2200, status: 'Paid', description: 'Testing services', invoiceNumber: 'TEST-2025-0115', glAccount: '5030' },
  { id: 'AP-144', vendorId: 'V005', vendorName: 'Bay Area Grow Supplies', date: '2025-01-18', dueDate: '2025-01-30', amount: 4560, paidAmount: 4560, status: 'Paid', description: 'Equipment maintenance', invoiceNumber: 'GROW-2025-0118', glAccount: '5000' },
];

// Invoices (Accounts Receivable)
export const mockInvoices: Invoice[] = [
  {
    id: 'INV-501',
    customerId: 'C001',
    customerName: 'Green Valley Dispensary',
    date: '2025-01-15',
    dueDate: '2025-01-28',
    amount: 12450,
    paidAmount: 0,
    status: 'Overdue',
    description: 'Cannabis wholesale order',
    invoiceNumber: 'INV-501',
    items: [
      { id: 'I1', description: 'Cannabis Flower - Premium', quantity: 10, rate: 850, amount: 8500, glAccount: '4000' },
      { id: 'I2', description: 'Cannabis Edibles - Mixed', quantity: 15, rate: 263.33, amount: 3950, glAccount: '4010' }
    ]
  },
  {
    id: 'INV-502',
    customerId: 'C002',
    customerName: 'Emerald Coast Cannabis',
    date: '2025-01-20',
    dueDate: '2025-02-04',
    amount: 8750,
    paidAmount: 0,
    status: 'Overdue',
    description: 'Mixed product order',
    invoiceNumber: 'INV-502',
    items: [
      { id: 'I3', description: 'Cannabis Concentrates', quantity: 5, rate: 1750, amount: 8750, glAccount: '4020' }
    ]
  },
  {
    id: 'INV-503',
    customerId: 'C003',
    customerName: 'Bay Area Wellness Center',
    date: '2025-01-25',
    dueDate: '2025-02-08',
    amount: 15600,
    paidAmount: 0,
    status: 'Sent',
    description: 'Monthly wholesale order',
    invoiceNumber: 'INV-503',
    items: [
      { id: 'I4', description: 'Medical Grade Flower', quantity: 12, rate: 950, amount: 11400, glAccount: '4000' },
      { id: 'I5', description: 'CBD Edibles', quantity: 14, rate: 300, amount: 4200, glAccount: '4010' }
    ]
  },
  {
    id: 'INV-504',
    customerId: 'C004',
    customerName: 'Central Valley Organics',
    date: '2025-01-28',
    dueDate: '2025-02-12',
    amount: 9840,
    paidAmount: 0,
    status: 'Sent',
    description: 'Organic products',
    invoiceNumber: 'INV-504',
    items: [
      { id: 'I6', description: 'Organic Flower', quantity: 8, rate: 1230, amount: 9840, glAccount: '4000' }
    ]
  },
  {
    id: 'INV-505',
    customerId: 'C005',
    customerName: 'Coastal Cannabis Collective',
    date: '2025-02-01',
    dueDate: '2025-02-15',
    amount: 18900,
    paidAmount: 0,
    status: 'Sent',
    description: 'Premium product order',
    invoiceNumber: 'INV-505',
    items: [
      { id: 'I7', description: 'Premium Flower', quantity: 15, rate: 890, amount: 13350, glAccount: '4000' },
      { id: 'I8', description: 'Premium Concentrates', quantity: 3, rate: 1850, amount: 5550, glAccount: '4020' }
    ]
  },
  {
    id: 'INV-506',
    customerId: 'C001',
    customerName: 'Green Valley Dispensary',
    date: '2025-02-02',
    dueDate: '2025-02-16',
    amount: 22450,
    paidAmount: 0,
    status: 'Sent',
    description: 'Large wholesale order',
    invoiceNumber: 'INV-506',
    items: [
      { id: 'I9', description: 'Mixed Flower', quantity: 20, rate: 780, amount: 15600, glAccount: '4000' },
      { id: 'I10', description: 'Edibles Variety Pack', quantity: 25, rate: 274, amount: 6850, glAccount: '4010' }
    ]
  }
];

// Cash Flow Data (30 days)
export const mockCashFlowData = [
  { date: '05 Jan', inflow: 18500, outflow: 12300, net: 6200 },
  { date: '06 Jan', inflow: 21400, outflow: 8900, net: 12500 },
  { date: '07 Jan', inflow: 15800, outflow: 15200, net: 600 },
  { date: '08 Jan', inflow: 24300, outflow: 18500, net: 5800 },
  { date: '09 Jan', inflow: 19700, outflow: 11200, net: 8500 },
  { date: '10 Jan', inflow: 22100, outflow: 9800, net: 12300 },
  { date: '11 Jan', inflow: 17900, outflow: 14500, net: 3400 },
  { date: '12 Jan', inflow: 26500, outflow: 19200, net: 7300 },
  { date: '13 Jan', inflow: 20300, outflow: 10500, net: 9800 },
  { date: '14 Jan', inflow: 23800, outflow: 12800, net: 11000 },
  { date: '15 Jan', inflow: 18200, outflow: 16700, net: 1500 },
  { date: '16 Jan', inflow: 25600, outflow: 14300, net: 11300 },
  { date: '17 Jan', inflow: 21700, outflow: 11900, net: 9800 },
  { date: '18 Jan', inflow: 19400, outflow: 13200, net: 6200 },
  { date: '19 Jan', inflow: 24900, outflow: 17800, net: 7100 },
  { date: '20 Jan', inflow: 22300, outflow: 10200, net: 12100 },
  { date: '21 Jan', inflow: 18900, outflow: 15600, net: 3300 },
  { date: '22 Jan', inflow: 27100, outflow: 19500, net: 7600 },
  { date: '23 Jan', inflow: 20800, outflow: 11400, net: 9400 },
  { date: '24 Jan', inflow: 23500, outflow: 13700, net: 9800 },
  { date: '25 Jan', inflow: 19100, outflow: 16200, net: 2900 },
  { date: '26 Jan', inflow: 26300, outflow: 14800, net: 11500 },
  { date: '27 Jan', inflow: 22600, outflow: 12100, net: 10500 },
  { date: '28 Jan', inflow: 20500, outflow: 14900, net: 5600 },
  { date: '29 Jan', inflow: 25800, outflow: 18600, net: 7200 },
  { date: '30 Jan', inflow: 23200, outflow: 11800, net: 11400 },
  { date: '31 Jan', inflow: 19800, outflow: 15300, net: 4500 },
  { date: '01 Feb', inflow: 28400, outflow: 20100, net: 8300 },
  { date: '02 Feb', inflow: 21900, outflow: 12600, net: 9300 },
  { date: '03 Feb', inflow: 24700, outflow: 14200, net: 10500 }
];

// P&L Data (6 months)
export const mockPLData = [
  { month: 'Sep', revenue: 225400, cogs: 68200, expenses: 52100, netIncome: 105100 },
  { month: 'Oct', revenue: 238900, cogs: 71800, expenses: 54300, netIncome: 112800 },
  { month: 'Nov', revenue: 242100, cogs: 73500, expenses: 55800, netIncome: 112800 },
  { month: 'Dec', revenue: 251800, cogs: 75900, expenses: 57200, netIncome: 118700 },
  { month: 'Jan', revenue: 245890, cogs: 76800, expenses: 57530, netIncome: 111560 },
  { month: 'Feb', revenue: 245890, cogs: 76800, expenses: 57530, netIncome: 111560 }
];

// AP Aging Data
export const mockAPAgingData = [
  { name: 'Current', value: 20000, count: 5 },
  { name: '1-30 Days', value: 12340, count: 3 },
  { name: '31-60 Days', value: 8450, count: 2 },
  { name: '61-90 Days', value: 4230, count: 1 },
  { name: '90+ Days', value: 210, count: 1 }
];

// AR Aging Data
export const mockARAgingData = [
  { name: 'Current', value: 86240, count: 18 },
  { name: '1-30 Days', value: 20110, count: 6 },
  { name: '31-60 Days', value: 12450, count: 4 },
  { name: '61-90 Days', value: 6850, count: 3 },
  { name: '90+ Days', value: 2800, count: 3 }
];
