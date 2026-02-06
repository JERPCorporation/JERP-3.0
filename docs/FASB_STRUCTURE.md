# FASB ASC Framework Structure

## Overview

JERP 3.0 uses the **FASB ASC (Financial Accounting Standards Board Accounting Standards Codification)** framework for account classification. This provides a standardized, professional approach to chart of accounts management that aligns with US GAAP.

## 🚨 Critical Requirements

- **FASB MAPPING IS REQUIRED** - Every account MUST be mapped to a FASB topic and subtopic
- **NO PRE-POPULATED ACCOUNTS** - Users create their own accounts with appropriate FASB mappings
- **FASB REFERENCE DATA IS AUTO-SEEDED** - All topics and subtopics are automatically available
- **280E COMPLIANCE** - IsCOGS and IsNonDeductible flags work alongside FASB for cannabis businesses

## Why FASB ASC?

The old `AccountSubType` enum was too limited with only ~30 options. FASB ASC provides:

✅ **650+ standardized classifications** across all accounting categories  
✅ **Professional financial reporting** aligned with US GAAP  
✅ **Industry-specific guidance** including cannabis-related topics  
✅ **Clear audit trail** with topic codes (e.g., "ASC 606-10")  
✅ **Future-proof** as new standards are added

## FASB ASC Structure

### Categories (200s-900s)

The FASB ASC is organized into 8 main categories:

| Category Code | Category Name | Description |
|--------------|---------------|-------------|
| 200 | Presentation | Financial statement presentation and format |
| 300 | Assets | Asset recognition, measurement, and disclosure |
| 400 | Liabilities | Liability recognition, measurement, and disclosure |
| 500 | Equity | Equity transactions and reporting |
| 600 | Revenue | Revenue recognition and measurement |
| 700 | Expenses | Expense recognition and measurement |
| 800 | Broad Transactions | Cross-cutting accounting issues |
| 900 | Industry | Industry-specific accounting guidance |

### Topics and Subtopics

Each category contains multiple **topics** (e.g., 305, 310, 606), and each topic has multiple **subtopics** (e.g., 10, 20, 30).

**Example:**
- Topic: `606` - Revenue from Contracts with Customers
- Subtopic: `10` - Overall
- Full Reference: **ASC 606-10**

## Complete FASB ASC Reference

### CATEGORY 1: PRESENTATION TOPICS (200s)

- **205** - Presentation of Financial Statements
  - 205-10: Overall
  - 205-20: Discontinued Operations
  - 205-30: Liquidation Basis of Accounting

- **210** - Balance Sheet
  - 210-10: Overall
  - 210-20: Offsetting

- **220** - Income Statement
  - 220-10: Overall
  - 220-20: Reporting Comprehensive Income

- **225** - Income Statement—Discontinued Operations
  - 225-10: Overall
  - 225-20: Discontinued Operations

- **230** - Statement of Cash Flows
  - 230-10: Overall

- **235** - Notes to Financial Statements
  - 235-10: Overall

- **250** - Accounting Changes and Error Corrections
  - 250-10: Overall

- **260** - Earnings Per Share
  - 260-10: Overall

- **270** - Interim Reporting
  - 270-10: Overall

- **272** - Limited Liability Entities
  - 272-10: Overall

- **273** - Corporate Joint Ventures
  - 273-10: Overall

- **274** - Personal Financial Statements
  - 274-10: Overall

- **280** - Segment Reporting
  - 280-10: Overall

### CATEGORY 2: ASSETS (300s)

- **305** - Cash and Cash Equivalents
  - 305-10: Overall

- **310** - Receivables
  - 310-10: Overall
  - 310-20: Nonrefundable Fees and Other Costs
  - 310-30: Loans and Debt Securities Acquired with Deteriorated Credit Quality

- **320** - Investments—Debt Securities (superseded)
  - 320-10: Overall

- **321** - Investments—Equity Securities
  - 321-10: Overall

- **323** - Investments—Equity Method and Joint Ventures
  - 323-10: Overall
  - 323-30: Investments in Tax Credit Structures

- **325** - Investments—Other
  - 325-10: Overall
  - 325-20: Cost Method Investments
  - 325-30: Investments in Insurance Contracts
  - 325-40: Beneficial Interests in Securitized Financial Assets

- **326** - Financial Instruments—Credit Losses
  - 326-10: Overall
  - 326-20: Measured at Amortized Cost
  - 326-30: Available-for-Sale Debt Securities

- **330** - Inventory
  - 330-10: Overall

- **340** - Other Assets and Deferred Costs
  - 340-10: Overall
  - 340-20: Capitalized Advertising Costs
  - 340-30: Insurance Contracts That Do Not Transfer Insurance Risk
  - 340-40: Deferred Revenue—Contracts with Customers

- **350** - Intangibles—Goodwill and Other
  - 350-10: Overall
  - 350-20: Goodwill
  - 350-30: General Intangibles Other than Goodwill
  - 350-40: Internal-Use Software
  - 350-50: Website Development Costs
  - 350-60: Goodwill and Other (Impairment)

- **360** - Property, Plant, and Equipment
  - 360-10: Overall
  - 360-20: Real Estate Sales

### CATEGORY 3: LIABILITIES (400s)

- **405** - Liabilities
  - 405-10: Overall
  - 405-20: Extinguishments of Liabilities
  - 405-30: Research and Development Arrangements
  - 405-40: Obligations Resulting from Joint and Several Liability Arrangements
  - 405-50: Derecognition

- **410** - Asset Retirement and Environmental Obligations
  - 410-10: Overall
  - 410-20: Asset Retirement Obligations
  - 410-30: Environmental Obligations

- **420** - Exit or Disposal Cost Obligations
  - 420-10: Overall

- **440** - Commitments
  - 440-10: Overall

- **450** - Contingencies
  - 450-10: Overall
  - 450-20: Loss Contingencies
  - 450-30: Gain Contingencies

- **460** - Guarantees
  - 460-10: Overall

- **470** - Debt
  - 470-10: Overall
  - 470-20: Debt with Conversion and Other Options
  - 470-30: Participating Mortgage Loans
  - 470-40: Product Financing Arrangements
  - 470-50: Modifications and Extinguishments
  - 470-60: Troubled Debt Restructurings by Debtors

- **480** - Distinguishing Liabilities from Equity
  - 480-10: Overall

### CATEGORY 4: EQUITY (500s)

- **505** - Equity
  - 505-10: Overall
  - 505-20: Stock Dividends and Stock Splits
  - 505-30: Treasury Stock
  - 505-40: Spin-offs and Reverse Spin-offs
  - 505-50: Equity-Based Payments to Non-Employees

### CATEGORY 5: REVENUE (600s)

- **605** - Revenue Recognition (superseded)
  - 605-10: Overall
  - 605-15: Products
  - 605-20: Services
  - 605-25: Multiple-Element Arrangements
  - 605-35: Construction-Type and Production-Type Contracts
  - 605-45: Principal Agent Considerations

- **606** - Revenue from Contracts with Customers
  - 606-10: Overall

- **610** - Other Income
  - 610-10: Overall
  - 610-20: Gains and Losses from the Derecognition of Nonfinancial Assets

### CATEGORY 6: EXPENSES (700s)

- **705** - Cost of Sales and Services
  - 705-10: Overall
  - 705-20: Recognition

- **710** - Compensation—General
  - 710-10: Overall

- **712** - Compensation—Nonretirement Postemployment Benefits
  - 712-10: Overall
  - 712-20: Defined Contribution Plans

- **715** - Compensation—Retirement Benefits
  - 715-10: Overall
  - 715-20: Defined Benefit Plans—General
  - 715-30: Defined Benefit Plans—Pension
  - 715-40: Defined Benefit Plans—Other Postretirement
  - 715-50: Defined Benefit Plans—Disclosures
  - 715-80: Multiemployer Plans

- **718** - Compensation—Stock Compensation
  - 718-10: Overall
  - 718-20: Awards Classified as Equity
  - 718-30: Awards Classified as Liabilities
  - 718-40: Employee Stock Ownership Plans

- **720** - Other Expenses
  - 720-10: Overall
  - 720-15: Start-Up Costs
  - 720-20: Insurance Costs
  - 720-25: Contributions Made
  - 720-30: Advertising Costs
  - 720-45: Fees Paid to Federal Government by Pharmaceutical Manufacturers

- **730** - Research and Development
  - 730-10: Overall
  - 730-20: Research and Development Arrangements

- **740** - Income Taxes
  - 740-10: Overall
  - 740-20: Intraperiod Tax Allocation
  - 740-30: Other Considerations or Special Areas

### CATEGORY 7: BROAD TRANSACTIONS (800s)

- **805** - Business Combinations
  - 805-10: Overall
  - 805-20: Identifiable Assets and Liabilities
  - 805-30: Goodwill or Gain from Bargain Purchase

- **810** - Consolidation
  - 810-10: Overall

- **815** - Derivatives and Hedging
  - 815-10: Overall
  - 815-20: Hedging—General
  - 815-25: Fair Value Hedges
  - 815-30: Cash Flow Hedges
  - 815-40: Contracts in Entity's Own Equity

- **820** - Fair Value Measurement
  - 820-10: Overall

- **825** - Financial Instruments
  - 825-10: Overall

- **830** - Foreign Currency Matters
  - 830-10: Overall
  - 830-20: Foreign Currency Transactions
  - 830-30: Translation of Financial Statements

- **835** - Interest
  - 835-10: Overall
  - 835-20: Capitalization of Interest
  - 835-30: Imputation of Interest

- **842** - Leases
  - 842-10: Overall
  - 842-20: Lessee
  - 842-30: Lessor
  - 842-40: Sale and Leaseback Transactions

- **845** - Nonmonetary Transactions
  - 845-10: Overall

- **850** - Related Party Disclosures
  - 850-10: Overall

- **855** - Subsequent Events
  - 855-10: Overall

- **860** - Transfers and Servicing
  - 860-10: Overall
  - 860-20: Sales of Financial Assets
  - 860-30: Secured Borrowing and Collateral

### CATEGORY 8: INDUSTRY TOPICS (900s)

- **920** - Entertainment—Broadcasters
  - 920-10: Overall
  - 920-350: Intangibles—Goodwill and Other

- **942** - Financial Services—Depository and Lending
  - 942-10: Overall

- **944** - Financial Services—Insurance
  - 944-10: Overall

- **954** - Health Care Entities
  - 954-10: Overall

- **958** - Not-for-Profit Entities
  - 958-10: Overall

- **970** - Real Estate—General
  - 970-10: Overall

- **980** - Regulated Operations
  - 980-10: Overall

## How to Create Accounts in JERP 3.0

### Step 1: Choose Account Type
Select the primary account type:
- Asset
- Liability
- Equity
- Revenue
- Expense

### Step 2: Select FASB Topic
Browse or search FASB topics in the appropriate category. For example:
- Cash accounts → Topic 305 (Cash and Cash Equivalents)
- Revenue accounts → Topic 606 (Revenue from Contracts)
- Payroll expense → Topic 710 (Compensation—General)

### Step 3: Select FASB Subtopic
Choose the most specific subtopic. Most commonly use:
- Subtopic 10: Overall (general classification)
- Subtopic 20+: More specific guidance

### Step 4: Set 280E Flags (Cannabis Businesses)
- **IsCOGS**: Mark as true for cost of goods sold (280E deductible)
- **IsNonDeductible**: Mark as true for selling expenses (280E non-deductible)

### Example: Creating a Cannabis Sales Account

```json
{
  "accountNumber": "4000",
  "accountName": "Cannabis Sales - Flower",
  "type": "Revenue",
  "fasbTopicId": "[606 Topic ID]",
  "fasbSubtopicId": "[606-10 Subtopic ID]",
  "isCOGS": false,
  "isNonDeductible": false,
  "taxCategory": "Cannabis Revenue"
}
```

Full FASB Reference: **ASC 606-10** (Revenue from Contracts with Customers - Overall)

### Example: Creating a COGS Account

```json
{
  "accountNumber": "5000",
  "accountName": "COGS - Cannabis Product",
  "type": "Expense",
  "fasbTopicId": "[705 Topic ID]",
  "fasbSubtopicId": "[705-10 Subtopic ID]",
  "isCOGS": true,
  "isNonDeductible": false,
  "taxCategory": "280E Deductible"
}
```

Full FASB Reference: **ASC 705-10** (Cost of Sales and Services - Overall)

## Common FASB Mappings for Cannabis Businesses

### Assets
- **Cash/Bank Accounts** → ASC 305-10 (Cash and Cash Equivalents)
- **Accounts Receivable** → ASC 310-10 (Receivables - Overall)
- **Inventory** → ASC 330-10 (Inventory - Overall)
- **Fixed Assets** → ASC 360-10 (Property, Plant, and Equipment)

### Liabilities
- **Accounts Payable** → ASC 405-10 (Liabilities - Overall)
- **Taxes Payable** → ASC 740-10 (Income Taxes - Overall)
- **Debt** → ASC 470-10 (Debt - Overall)

### Equity
- **Owner's Equity** → ASC 505-10 (Equity - Overall)
- **Retained Earnings** → ASC 505-10 (Equity - Overall)

### Revenue
- **Product Sales** → ASC 606-10 (Revenue from Contracts with Customers)

### COGS (280E Deductible)
- **Product Costs** → ASC 705-10 (Cost of Sales - Overall)
- **Direct Labor** → ASC 705-10 (Cost of Sales - Overall)
- **Packaging** → ASC 705-10 (Cost of Sales - Overall)

### Expenses (280E Non-Deductible)
- **Payroll** → ASC 710-10 (Compensation—General)
- **Rent** → ASC 842-20 (Leases - Lessee) or ASC 720-10 (Other Expenses)
- **Utilities** → ASC 720-10 (Other Expenses - Overall)
- **Marketing** → ASC 720-30 (Advertising Costs)
- **Insurance** → ASC 720-20 (Insurance Costs)

## Cannabis 280E Compliance

FASB mapping works alongside 280E compliance flags:

1. **IsCOGS = true**: Expenses directly allocable to product (280E deductible)
   - Use FASB ASC 705 (Cost of Sales)
   
2. **IsNonDeductible = true**: Selling and business expenses (280E non-deductible)
   - Use FASB ASC 710 (Payroll), 720 (Other Expenses), etc.

3. **Revenue accounts**: Neither flag (always reported)
   - Use FASB ASC 606 (Revenue from Contracts)

## API Endpoints

### Get All FASB Topics
```
GET /api/v1/finance/fasb-topics?category=Assets
```

### Get Subtopics for a Topic
```
GET /api/v1/finance/fasb-topics/{topicId}/subtopics
```

### Create Account with FASB Mapping
```
POST /api/v1/finance/accounts
{
  "accountNumber": "1000",
  "accountName": "Cash - Operating",
  "type": "Asset",
  "fasbTopicId": "guid-for-305",
  "fasbSubtopicId": "guid-for-305-10",
  "isCOGS": false,
  "isNonDeductible": false
}
```

## Benefits of FASB ASC Framework

### For Cannabis Businesses
✅ Professional chart of accounts aligned with US GAAP  
✅ Clear 280E deductibility tracking with FASB context  
✅ Audit-ready financial statements  
✅ Industry-standard reporting for investors/lenders  

### For All Businesses
✅ Standardized account classification  
✅ Consistent financial reporting  
✅ Easy to train accountants (they know FASB)  
✅ Future-proof as accounting standards evolve  

## Migration from Old AccountSubType

The old `AccountSubType` enum has been completely removed. All accounts now require:
- FASB Topic ID (required)
- FASB Subtopic ID (required)
- FASB Reference string (auto-calculated, e.g., "ASC 606-10")

**No pre-populated accounts** - users create accounts as needed with appropriate FASB mappings.

## Support

For questions about FASB ASC classification:
- Consult the official FASB website: https://asc.fasb.org
- Work with your CPA for proper account classification
- Contact JERP support: support@jerp.io

---

**JERP 3.0** - Professional ERP for Cannabis Businesses  
© 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
