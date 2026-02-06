# Database Migration for FASB Framework

## Overview

This document describes the database migration needed to transition from the old `AccountSubType` enum to the FASB ASC framework.

## Migration Name

`ReplacedAccountSubTypeWithFASB`

## What Changed

### Tables Added
- `FASBTopics` - Already exists, seeded with 91 FASB topics
- `FASBSubtopics` - Already exists, seeded with 650+ subtopics

### Accounts Table Changes

#### Columns Removed
- `SubType` (enum/string) - REMOVED completely

#### Columns Modified
- `FASBTopicId` - Changed from `NULLABLE` to `NOT NULL` (REQUIRED)
- `FASBSubtopicId` - Changed from `NULLABLE` to `NOT NULL` (REQUIRED)
- `FASBReference` - Remains nullable (auto-calculated)

#### Foreign Keys Modified
- FK to FASBTopics - Changed from `ON DELETE SET NULL` to `ON DELETE RESTRICT`
- FK to FASBSubtopics - Changed from `ON DELETE SET NULL` to `ON DELETE RESTRICT`

## Migration Command

```bash
cd src/JERP.Infrastructure
dotnet ef migrations add ReplacedAccountSubTypeWithFASB --startup-project ../JERP.Api
dotnet ef database update --startup-project ../JERP.Api
```

## Important Notes

### ⚠️ Breaking Changes

1. **All existing accounts MUST be updated** to have valid FASB mappings before this migration can run
2. **Account seeding has been REMOVED** - The old `ChartOfAccountsSeedService` no longer creates accounts
3. **FASB data is auto-seeded** - All topics and subtopics are automatically available

### Data Migration Strategy

**Option 1: Fresh Installation (Recommended)**
- Drop and recreate database
- FASB reference data will be seeded automatically
- Users create their own accounts with FASB mappings

**Option 2: Existing Data Migration**
If you have existing accounts, you need to:

1. **Before Migration**: Map existing accounts to FASB codes
   - Cash accounts → ASC 305-10
   - Receivables → ASC 310-10
   - Inventory → ASC 330-10
   - Revenue → ASC 606-10
   - COGS → ASC 705-10
   - Payroll → ASC 710-10
   - Other expenses → ASC 720-10

2. **Create Custom Migration Script** to update existing accounts:
```sql
-- Example: Update cash accounts
UPDATE Accounts 
SET FASBTopicId = (SELECT Id FROM FASBTopics WHERE TopicCode = '305'),
    FASBSubtopicId = (SELECT Id FROM FASBSubtopics WHERE FASBTopicId = (SELECT Id FROM FASBTopics WHERE TopicCode = '305') AND SubtopicCode = '10'),
    FASBReference = 'ASC 305-10'
WHERE SubType = 'Cash';

-- Repeat for all SubType values...
```

3. **Then Run Migration** to drop SubType column and enforce required FASB fields

### Old SubType to FASB Mapping

| Old SubType | FASB Topic | FASB Subtopic | Reference |
|-------------|------------|---------------|-----------|
| Cash | 305 | 10 | ASC 305-10 |
| BankAccount | 305 | 10 | ASC 305-10 |
| AccountsReceivable | 310 | 10 | ASC 310-10 |
| Inventory | 330 | 10 | ASC 330-10 |
| PrepaidExpense | 340 | 10 | ASC 340-10 |
| FixedAsset | 360 | 10 | ASC 360-10 |
| OtherAsset | 340 | 10 | ASC 340-10 |
| AccountsPayable | 405 | 10 | ASC 405-10 |
| CreditCard | 405 | 10 | ASC 405-10 |
| TaxPayable | 740 | 10 | ASC 740-10 |
| PayrollLiability | 710 | 10 | ASC 710-10 |
| LongTermDebt | 470 | 10 | ASC 470-10 |
| OtherLiability | 405 | 10 | ASC 405-10 |
| OwnersEquity | 505 | 10 | ASC 505-10 |
| RetainedEarnings | 505 | 10 | ASC 505-10 |
| Distributions | 505 | 10 | ASC 505-10 |
| ProductSales | 606 | 10 | ASC 606-10 |
| ServiceRevenue | 606 | 10 | ASC 606-10 |
| OtherIncome | 610 | 10 | ASC 610-10 |
| COGS | 705 | 10 | ASC 705-10 |
| PayrollExpense | 710 | 10 | ASC 710-10 |
| PayrollTaxExpense | 710 | 10 | ASC 710-10 |
| RentExpense | 842 | 20 | ASC 842-20 |
| UtilitiesExpense | 720 | 10 | ASC 720-10 |
| MarketingExpense | 720 | 30 | ASC 720-30 |
| OfficeExpense | 720 | 10 | ASC 720-10 |
| InsuranceExpense | 720 | 20 | ASC 720-20 |
| ProfessionalFees | 720 | 10 | ASC 720-10 |
| OtherExpense | 720 | 10 | ASC 720-10 |

## Configuration Changes

### Entity Framework Configurations Updated

1. **AccountConfiguration.cs**
   - Removed SubType property configuration
   - Made FASB relationships required with RESTRICT delete behavior

2. **FASBTopicConfiguration.cs**
   - Changed Accounts relationship to RESTRICT (was SET NULL)

3. **FASBSubtopicConfiguration.cs**
   - Changed Accounts relationship to RESTRICT (was SET NULL)

## Testing the Migration

### Pre-Migration Checklist
- [ ] Backup production database
- [ ] Verify FASB data is seeded (should have 91 topics)
- [ ] Map all existing accounts to FASB codes (if upgrading)
- [ ] Test migration on staging environment

### Post-Migration Verification
- [ ] Verify FASB tables have data (91+ topics, 650+ subtopics)
- [ ] Verify SubType column is removed from Accounts table
- [ ] Verify FASBTopicId and FASBSubtopicId are NOT NULL
- [ ] Verify foreign key constraints are RESTRICT
- [ ] Test creating new accounts with FASB mappings
- [ ] Test account queries include FASB data

## Rollback Strategy

If migration fails:

1. **Restore database backup**
2. **Revert code changes** to previous commit
3. **Investigate issues** before re-attempting

## Support

For migration assistance:
- Review `docs/FASB_STRUCTURE.md` for FASB reference
- Contact: support@jerp.io

---

**JERP 3.0** - Professional ERP for Cannabis Businesses  
© 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
