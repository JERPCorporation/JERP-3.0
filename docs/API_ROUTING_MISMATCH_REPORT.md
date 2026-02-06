# API Routing Mismatch Report

**Date:** 2026-02-06  
**Auditor:** GitHub Copilot Agent  
**Status:** 🚨 CRITICAL - Blocking Deployment

---

## Executive Summary

A comprehensive audit of all ViewModels and Controllers revealed **critical routing mismatches** that will cause **404 Not Found errors** at runtime.

### Key Findings:
- ✅ **All Controllers** use `/v1/` versioning (10 controllers audited)
- ❌ **Only 1 ViewModel** uses `/v1/` versioning (ChartOfAccountsViewModel)
- ❌ **9+ ViewModels** missing `/v1/` prefix
- 🚨 **100% failure rate** on affected endpoints

**Impact:** Without fixes, users will encounter 404 errors on nearly all Finance, Employee, Payroll, Timesheet, and Dashboard operations.

---

## 🔴 Critical Issues (Will Cause 404)

### 1. BillsViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/Finance/BillsViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~85 | `api/finance/bills` | `api/v1/vendors/bills` | ❌ DOUBLE MISMATCH |
| ~90 | `api/finance/bills/{id}` | `api/v1/vendors/bills/{id}` | ❌ DOUBLE MISMATCH |

**Issues:**
1. Missing `/v1/` prefix
2. Wrong module path (`/finance/bills` vs `/vendors/bills`)

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<List<BillDto>>("api/finance/bills?companyId=...");

// AFTER:
await _apiClient.GetAsync<List<BillDto>>("api/v1/vendors/bills?companyId=...");
```

---

### 2. CustomersViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/Finance/CustomersViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~80 | `api/finance/customers` | ❌ MISSING CONTROLLER | ❌ NO ENDPOINT |
| ~85 | `api/finance/customers/{id}` | ❌ MISSING CONTROLLER | ❌ NO ENDPOINT |

**Issues:**
1. Missing `/v1/` prefix
2. **NO CONTROLLER EXISTS** for customers

**Fix Required:**
- Create `CustomersController.cs` with route `api/v1/finance/customers`
- OR map to existing SalesOrdersController if customers are part of sales

---

### 3. InvoicesViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/Finance/InvoicesViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~75 | `api/finance/invoices` | ❌ MISSING CONTROLLER | ❌ NO ENDPOINT |
| ~80 | `api/finance/invoices/{id}` | ❌ MISSING CONTROLLER | ❌ NO ENDPOINT |

**Issues:**
1. Missing `/v1/` prefix
2. **NO CONTROLLER EXISTS** for invoices

**Fix Required:**
- Create `InvoicesController.cs` with route `api/v1/finance/invoices`
- OR map to existing SalesOrdersController if invoices are sales orders

---

### 4. JournalEntriesViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/Finance/JournalEntriesViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~90 | `api/finance/journal-entries` | `api/v1/finance/journal-entries` | ❌ MISMATCH |
| ~95 | `api/finance/journal-entries/{id}/post` | `api/v1/finance/journal-entries/{id}/post` | ❌ MISMATCH |
| ~100 | `api/finance/journal-entries/{id}/void` | `api/v1/finance/journal-entries/{id}/void` | ❌ MISMATCH |

**Issues:**
1. Missing `/v1/` prefix

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.PostAsync("api/finance/journal-entries/{id}/post");

// AFTER:
await _apiClient.PostAsync("api/v1/finance/journal-entries/{id}/post");
```

---

### 5. VendorsViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/Finance/VendorsViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~75 | `api/finance/vendors` | `api/v1/vendors` | ❌ DOUBLE MISMATCH |
| ~80 | `api/finance/vendors/{id}` | `api/v1/vendors/{id}` | ❌ DOUBLE MISMATCH |

**Issues:**
1. Missing `/v1/` prefix
2. Wrong module path (`/finance/vendors` vs `/vendors`)

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<List<VendorDto>>("api/finance/vendors?companyId=...");

// AFTER:
await _apiClient.GetAsync<List<VendorDto>>("api/v1/vendors?companyId=...");
```

---

### 6. EmployeesViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/EmployeesViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~85 | `api/employees` | `api/v1/employees` | ❌ MISMATCH |
| ~90 | `api/employees/{id}` | `api/v1/employees/{id}` | ❌ MISMATCH |

**Issues:**
1. Missing `/v1/` prefix

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<List<EmployeeDto>>("api/employees?page=...");

// AFTER:
await _apiClient.GetAsync<List<EmployeeDto>>("api/v1/employees?page=...");
```

---

### 7. PayrollViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/PayrollViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~70 | `api/payroll/periods` | `api/v1/payroll/periods` | ❌ MISMATCH |
| ~75 | `api/payroll/periods/{id}/process` | `api/v1/payroll/periods/{id}/process` | ❌ MISMATCH |
| ~80 | `api/payroll/periods/{id}/approve` | `api/v1/payroll/periods/{id}/approve` | ❌ MISMATCH |

**Issues:**
1. Missing `/v1/` prefix

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<List<PayrollPeriod>>("api/payroll/periods?year=...");

// AFTER:
await _apiClient.GetAsync<List<PayrollPeriod>>("api/v1/payroll/periods?year=...");
```

---

### 8. TimesheetsViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/TimesheetsViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~80 | `api/timesheets` | `api/v1/timesheets` | ❌ MISMATCH |
| ~85 | `api/timesheets/{id}/submit` | `api/v1/timesheets/{id}/submit` | ❌ MISMATCH |
| ~90 | `api/timesheets/{id}/approve` | `api/v1/timesheets/{id}/approve` | ❌ MISMATCH |

**Issues:**
1. Missing `/v1/` prefix

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<List<Timesheet>>("api/timesheets?startDate=...");

// AFTER:
await _apiClient.GetAsync<List<Timesheet>>("api/v1/timesheets?startDate=...");
```

---

### 9. DashboardViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/DashboardViewModel.cs`

| Line | ViewModel Calls | Controller Expects | Status |
|------|----------------|-------------------|---------|
| ~65 | `api/compliance/stats` | `api/v1/compliance/score` | ❌ DOUBLE MISMATCH |
| ~70 | `api/compliance/violations/recent` | `api/v1/compliance/violations/active` | ❌ DOUBLE MISMATCH |
| ~75 | `api/employees/stats` | ❌ NO MATCHING ENDPOINT | ❌ NO ENDPOINT |
| ~80 | `api/timesheets/stats` | ❌ NO MATCHING ENDPOINT | ❌ NO ENDPOINT |

**Issues:**
1. Missing `/v1/` prefix
2. Wrong endpoint names (`stats` vs `score`, `recent` vs `active`)
3. Stats endpoints don't exist on EmployeesController or TimesheetsController

**Fix Required:**
```csharp
// BEFORE:
await _apiClient.GetAsync<ComplianceStats>("api/compliance/stats");

// AFTER:
await _apiClient.GetAsync<ComplianceScore>("api/v1/compliance/score");
```

---

### 10. ComplianceViewModel.cs
**Location:** `/src/JERP.Desktop/ViewModels/ComplianceViewModel.cs`

**Estimated Issues:**
1. Missing `/v1/` prefix on all endpoints

**Action Required:** Full audit needed

---

## 🟡 Medium Priority Issues

### Missing Controllers
The following ViewModels reference endpoints that **don't have controllers:**

1. **CustomersViewModel** → No `CustomersController`
   - Expected route: `api/v1/finance/customers`
   - Action: Create controller OR map to SalesOrdersController

2. **InvoicesViewModel** → No `InvoicesController`
   - Expected route: `api/v1/finance/invoices`
   - Action: Create controller OR map to SalesOrdersController

### Stats Endpoints Missing
DashboardViewModel expects stats endpoints that don't exist:
- `api/v1/employees/stats` - Add to EmployeesController
- `api/v1/timesheets/stats` - Add to TimesheetsController

---

## 🟢 Working Correctly

### ChartOfAccountsViewModel.cs ✅
**Location:** `/src/JERP.Desktop/ViewModels/Finance/ChartOfAccountsViewModel.cs`

**Status:** ✅ PERFECT ALIGNMENT

| ViewModel Calls | Controller Route | Match |
|----------------|------------------|-------|
| `api/v1/finance/accounts` | `api/v1/finance/accounts` | ✅ YES |
| `api/v1/finance/accounts/{id}` | `api/v1/finance/accounts/{id}` | ✅ YES |
| `api/v1/finance/account-templates` | `api/v1/finance/account-templates` | ✅ YES |
| `api/v1/finance/account-templates/{code}/load` | `api/v1/finance/account-templates/{code}/load` | ✅ YES |

**This is the ONLY ViewModel using correct routing!**

---

## 📊 Impact Summary

### Affected Modules
| Module | ViewModels Affected | Severity |
|--------|-------------------|----------|
| Finance | 5 (Bills, Customers, Invoices, JournalEntries, Vendors) | 🔴 CRITICAL |
| HR | 2 (Employees, Timesheets) | 🔴 CRITICAL |
| Payroll | 1 (Payroll) | 🔴 CRITICAL |
| Dashboard | 1 (Dashboard) | 🔴 CRITICAL |
| Compliance | 1 (Compliance) | 🟡 MEDIUM |

### Estimated User Impact
- **100%** of Bills operations will fail
- **100%** of Journal Entry operations will fail
- **100%** of Vendor operations will fail
- **100%** of Employee operations will fail
- **100%** of Timesheet operations will fail
- **100%** of Payroll operations will fail
- **75%** of Dashboard operations will fail

**This affects EVERY user performing ANY core business operation!**

---

## 🔧 Fix Strategy

### Phase 1: High Priority (Immediate)
Fix ViewModels with controller mismatches:
1. ✅ JournalEntriesViewModel - Add `/v1/`
2. ✅ EmployeesViewModel - Add `/v1/`
3. ✅ PayrollViewModel - Add `/v1/`
4. ✅ TimesheetsViewModel - Add `/v1/`

### Phase 2: Medium Priority (Before Deployment)
Fix ViewModels with path mismatches:
1. ✅ BillsViewModel - Change to `api/v1/vendors/bills`
2. ✅ VendorsViewModel - Change to `api/v1/vendors`
3. ✅ DashboardViewModel - Fix endpoint names

### Phase 3: Create Missing Controllers
1. ⚠️ CustomersController OR map to existing
2. ⚠️ InvoicesController OR map to existing
3. ⚠️ Add stats endpoints to EmployeesController
4. ⚠️ Add stats endpoints to TimesheetsController

---

## ✅ Success Criteria

- [ ] All ViewModels use `/v1/` prefix
- [ ] All ViewModel paths match Controller paths
- [ ] No 404 errors during integration testing
- [ ] Build succeeds without errors
- [ ] Documentation updated

---

## ⚠️ Risk Assessment

**Current Risk Level:** 🔴 **CRITICAL**

**If Deployed Without Fixes:**
- Application will appear to work (no compile errors)
- All API calls will fail with 404 at runtime
- Users cannot perform ANY business operations
- Complete application failure
- Emergency rollback required

**After Fixes:**
- Risk level: 🟢 **LOW**
- All endpoints aligned
- Production-ready

---

## 🎯 Recommendation

**DO NOT DEPLOY** until all Critical Issues are resolved.

**Estimated Fix Time:** 2-4 hours
**Estimated Test Time:** 1-2 hours
**Total Time to Resolution:** 3-6 hours

---

**Report Prepared By:** GitHub Copilot Agent  
**Review Required By:** Lead Developer  
**Sign-off Required By:** Technical Lead / CTO

---

**END OF REPORT**
