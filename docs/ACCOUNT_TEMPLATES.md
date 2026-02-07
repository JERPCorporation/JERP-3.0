# Account Template System

## Overview

The Account Template System provides pre-configured Chart of Accounts templates that allow users to quickly bootstrap their accounting system with industry-specific, FASB-compliant account structures.

## Business Value

- **Saves Time**: Set up your entire chart of accounts in seconds instead of hours
- **Reduces Errors**: Pre-configured FASB mappings ensure compliance
- **Industry-Specific**: Templates tailored for different business types
- **Cannabis 280E**: Special template with proper tax treatment for cannabis businesses

## Available Templates

### 1. Basic Business (50 accounts)
**Code**: `basic-business`  
**Industry**: General  
**Best For**: Service businesses, consultants, freelancers

**Account Breakdown**:
- Assets: 15 accounts
- Liabilities: 10 accounts
- Equity: 5 accounts
- Revenue: 10 accounts
- Expenses: 10 accounts

**Key Features**:
- Simple account structure
- Essential business accounts
- Perfect for small service businesses
- Cash-basis accounting friendly

### 2. Retail (75 accounts)
**Code**: `retail`  
**Industry**: Retail  
**Best For**: Retail stores, e-commerce, dispensaries

**Account Breakdown**:
- Assets: 25 accounts (includes inventory accounts)
- Liabilities: 15 accounts
- Equity: 5 accounts
- Revenue: 15 accounts (multiple revenue streams)
- Expenses: 15 accounts

**Key Features**:
- Inventory management accounts
- Point of Sale accounts
- Gift card and loyalty program tracking
- Cost of Goods Sold tracking
- Sales tax compliance

### 3. Manufacturing (100 accounts)
**Code**: `manufacturing`  
**Industry**: Manufacturing  
**Best For**: Product manufacturers, assembly operations

**Account Breakdown**:
- Assets: 30 accounts (3-tier inventory)
- Liabilities: 20 accounts
- Equity: 10 accounts
- Revenue: 10 accounts
- Expenses: 30 accounts (detailed manufacturing costs)

**Key Features**:
- Three-tier inventory system (Raw Materials, WIP, Finished Goods)
- Manufacturing overhead tracking
- Factory wages and direct labor
- Equipment and machinery accounts
- R&D expense tracking
- Quality control costs

### 4. Construction (90 accounts)
**Code**: `construction`  
**Industry**: Construction  
**Best For**: General contractors, subcontractors

**Account Breakdown**:
- Assets: 25 accounts
- Liabilities: 20 accounts
- Equity: 10 accounts
- Revenue: 15 accounts
- Expenses: 20 accounts

**Key Features**:
- Construction in Progress (CIP) tracking
- Job costing accounts (labor, materials, subcontractors)
- Retainage tracking (receivable & payable)
- Equipment and tools accounts
- ASC 606-10 revenue recognition
- Progress billing support

### 5. Cannabis Business (120 accounts)
**Code**: `cannabis`  
**Industry**: Cannabis  
**Best For**: Licensed cannabis operations (cultivation, manufacturing, retail)

**Account Breakdown**:
- Assets: 35 accounts (multi-stage inventory)
- Liabilities: 20 accounts
- Equity: 10 accounts
- Revenue: 20 accounts (product types)
- Expenses: 35 accounts (280E compliance)

**Key Features**:
- **IRC 280E Compliant**: Proper separation of deductible COGS vs non-deductible expenses
- 20 COGS accounts marked as `"irc280e": "deductible"`
- 32 operating expense accounts marked as `"irc280e": "non-deductible"`
- Multi-stage inventory: Seeds, Plants, Trim, Flower, Concentrates, Edibles
- Cannabis-specific taxes (excise, cultivation tax)
- Product-type revenue tracking (flower, concentrates, edibles, topicals)
- Compliance-ready for state and federal reporting

## FASB Mappings

All templates use proper FASB Accounting Standards Codification (ASC) mappings:

| FASB Code | Topic | Usage |
|-----------|-------|-------|
| ASC 305-10 | Cash and Cash Equivalents | Bank accounts, petty cash |
| ASC 310-10 | Receivables | Accounts receivable, notes receivable |
| ASC 330-10 | Inventory | Raw materials, WIP, finished goods |
| ASC 340-10 | Other Assets and Deferred Costs | Prepaid expenses, deferred costs |
| ASC 350-10 | Intangibles | Patents, trademarks, goodwill |
| ASC 360-10 | Property, Plant & Equipment | Fixed assets, depreciation |
| ASC 405-10 | Liabilities | Accounts payable, accrued expenses |
| ASC 470-10 | Debt | Loans, notes payable |
| ASC 505-10 | Equity | Owner's equity, retained earnings, stock |
| ASC 606-10 | Revenue Recognition | Revenue from contracts with customers |
| ASC 710-10 | Compensation | Salaries, wages, benefits |
| ASC 720-10 | Other Expenses | Operating expenses |
| ASC 835-10 | Interest | Interest income and expense |
| ASC 842-10 | Leases | Lease assets and liabilities |

## API Usage

### 1. List Available Templates

```http
GET /api/v1/finance/account-templates
Authorization: Bearer {token}
```

**Response**:
```json
[
  {
    "code": "basic-business",
    "name": "Basic Business",
    "description": "Perfect for service businesses, consultants, and freelancers",
    "industry": "General",
    "accountCount": 50,
    "breakdown": {
      "assets": 15,
      "liabilities": 10,
      "equity": 5,
      "revenue": 10,
      "expenses": 10
    }
  },
  ...
]
```

### 2. Get Template Details

```http
GET /api/v1/finance/account-templates/{templateCode}
Authorization: Bearer {token}
```

**Example**:
```http
GET /api/v1/finance/account-templates/cannabis
```

**Response**:
```json
{
  "code": "cannabis",
  "name": "Cannabis Business",
  "description": "IRC 280E compliant template for licensed cannabis operations",
  "industry": "Cannabis",
  "accountCount": 120,
  "breakdown": {
    "assets": 35,
    "liabilities": 20,
    "equity": 10,
    "revenue": 20,
    "expenses": 35
  },
  "accounts": [
    {
      "accountNumber": "1000",
      "accountName": "Cash in Bank",
      "type": "Asset",
      "fasbTopicCode": "305",
      "fasbSubtopicCode": "10",
      "fasbReference": "ASC 305-10",
      "normalBalance": "Debit",
      "description": "Primary operating bank account"
    },
    ...
  ]
}
```

### 3. Load Template into Company

```http
POST /api/v1/finance/account-templates/{templateCode}/load
Authorization: Bearer {token}
Content-Type: application/json

{
  "companyId": "guid-here",
  "overwriteExisting": false
}
```

**Example**:
```http
POST /api/v1/finance/account-templates/basic-business/load
Content-Type: application/json

{
  "companyId": "123e4567-e89b-12d3-a456-426614174000",
  "overwriteExisting": false
}
```

**Response**:
```json
{
  "accountsCreated": 50,
  "accountsSkipped": 0,
  "errors": [],
  "success": true
}
```

**Error Response** (if template has issues):
```json
{
  "accountsCreated": 45,
  "accountsSkipped": 3,
  "errors": [
    "FASB Topic 999 not found for account 5999",
    "Invalid account type 'InvalidType' for account 6000"
  ],
  "success": false
}
```

## Cannabis 280E Compliance

The cannabis template is specifically designed for IRC 280E compliance:

### What is IRC 280E?

IRC Section 280E prohibits businesses trafficking in Schedule I or II substances from deducting ordinary business expenses. However, **Cost of Goods Sold (COGS)** is still deductible.

### How the Template Helps

The cannabis template clearly separates:

1. **Deductible COGS Accounts** (20 accounts)
   - Marked with `"irc280e": "deductible"` metadata
   - Examples:
     - Inventory purchases (seeds, plants, trim)
     - Direct cultivation labor
     - Direct manufacturing costs
     - Packaging for products
     - Freight-in

2. **Non-Deductible Operating Expenses** (32 accounts)
   - Marked with `"irc280e": "non-deductible"` metadata
   - Examples:
     - Administrative salaries
     - Marketing and advertising
     - Professional fees (legal, accounting)
     - Rent (non-cultivation/manufacturing)
     - Office supplies
     - Insurance

### Best Practices for 280E Compliance

1. **Use the cannabis template** - Pre-configured with proper COGS categorization
2. **Track time carefully** - Employees working on COGS activities vs. non-COGS
3. **Allocate overhead** - Manufacturing/cultivation overhead is deductible
4. **Document everything** - IRS scrutiny is high in cannabis
5. **Use metadata** - The `irc280e` metadata field helps with tax reporting

## Custom Templates

### Creating a Custom Template

1. Create a JSON file following the template structure
2. Place it in `src/JERP.Application/Data/Templates/`
3. Add the template summary to `GetAvailableTemplatesAsync()` in `AccountTemplateService.cs`

**Template Structure**:
```json
{
  "code": "my-custom-template",
  "name": "My Custom Template",
  "description": "Description of the template",
  "industry": "Industry Name",
  "accountCount": 50,
  "breakdown": {
    "assets": 15,
    "liabilities": 10,
    "equity": 5,
    "revenue": 10,
    "expenses": 10
  },
  "accounts": [
    {
      "accountNumber": "1000",
      "accountName": "Account Name",
      "type": "Asset",
      "fasbTopicCode": "305",
      "fasbSubtopicCode": "10",
      "fasbReference": "ASC 305-10",
      "normalBalance": "Debit",
      "description": "Account description",
      "metadata": {
        "custom_field": "value"
      }
    }
  ]
}
```

### Template Guidelines

1. **Account Numbers**: Use standard numbering convention
   - 1000-1999: Assets
   - 2000-2999: Liabilities
   - 3000-3999: Equity
   - 4000-4999: Revenue
   - 5000-5999: Expenses

2. **FASB Mappings**: Always include valid FASB topic and subtopic codes

3. **Normal Balances**:
   - Assets: Debit (except contra-assets like Accumulated Depreciation)
   - Liabilities: Credit
   - Equity: Credit (except Owner's Drawings)
   - Revenue: Credit (except contra-revenue like Sales Returns)
   - Expenses: Debit

4. **Metadata**: Use for custom fields like 280E classification, department codes, etc.

## Deployment

### Template File Deployment

Templates must be deployed with the application:

1. **Development**: Templates are in `src/JERP.Application/Data/Templates/`
2. **Production**: Templates should be copied to `{AppDirectory}/Data/Templates/`

### Docker Deployment

Add to Dockerfile:
```dockerfile
COPY src/JERP.Application/Data /app/Data
```

### Manual Deployment

```bash
mkdir -p /path/to/app/Data/Templates
cp src/JERP.Application/Data/Templates/*.json /path/to/app/Data/Templates/
```

## Troubleshooting

### Template Not Found

**Error**: `Template 'xyz' not found`

**Solutions**:
1. Check template file exists in `Data/Templates/` directory
2. Verify file name matches template code (e.g., `basic-business.json`)
3. Check file permissions (readable by application)

### FASB Topic Not Found

**Error**: `FASB Topic XXX not found for account YYYY`

**Solutions**:
1. Ensure FASB data is seeded (runs automatically on startup)
2. Verify the FASB topic code exists in the database
3. Check that the FASB topic hasn't been marked as superseded

### Account Creation Errors

**Error**: `Invalid account type 'XYZ' for account 1234`

**Solutions**:
1. Account type must be one of: `Asset`, `Liability`, `Equity`, `Revenue`, `Expense`
2. Check JSON is valid and properly formatted
3. Ensure all required fields are present

## Security

- All endpoints require authentication via `[Authorize]` attribute
- Only users with proper permissions can load templates
- Template loading is audited in the system logs
- Company ID validation ensures users can only load templates to their authorized companies

## Performance

- Template listing is fast (in-memory data)
- Template details are loaded on-demand from JSON files
- Template loading uses batch insert for efficiency
- FASB mappings are cached during loading to reduce database queries

## Future Enhancements

- [ ] Template versioning
- [ ] Custom template builder UI
- [ ] Template import/export
- [ ] Template sharing marketplace
- [ ] Multi-language support for account names
- [ ] Industry-specific KPIs embedded in templates
- [ ] Automated FASB code validation
- [ ] Template preview before loading

## Support

For questions or issues with the Account Template System:
- Email: ichbincesartobar@yahoo.com
- Documentation: https://docs.jerp.io
- GitHub Issues: https://github.com/ninoyerbas/JERP-3.0/issues

---

**Last Updated**: 2026-02-05  
**Version**: 3.0
