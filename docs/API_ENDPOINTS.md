# JERP 3.0 API Endpoint Reference

**Last Updated:** 2026-02-06  
**Status:** ✅ Standardized with `/v1/` versioning

---

## 🎯 Versioning Standard

All API endpoints follow this pattern:
```
api/v1/{module}/{resource}
```

**Why `/v1/`?**
- ✅ API versioning support for future v2, v3
- ✅ Industry standard REST API practice
- ✅ Backward compatibility protection

---

## 🔐 Authentication

### Auth Endpoints
- **POST** `api/v1/auth/login` - User login
- **POST** `api/v1/auth/refresh` - Refresh access token
- **POST** `api/v1/auth/logout` - User logout
- **GET** `api/v1/auth/me` - Get current user profile

**Query Parameters:** None  
**Authorization:** Not required for login, required for all others

---

## 📊 Dashboard

### Dashboard Metrics
- **GET** `api/v1/dashboard/kpis` - Get key performance indicators
- **GET** `api/v1/dashboard/alerts` - Get system alerts
- **GET** `api/v1/dashboard/overview` - Get dashboard overview
- **GET** `api/v1/dashboard/payroll-metrics` - Get payroll metrics
- **GET** `api/v1/dashboard/compliance-trend` - Get compliance trend data
- **GET** `api/v1/dashboard/employee-distribution` - Get employee distribution
- **GET** `api/v1/dashboard/pending-approvals` - Get pending approvals

**Query Parameters:** `companyId` (required)  
**Authorization:** Required

---

## 👥 Employees

### Employee Management
- **GET** `api/v1/employees` - List all employees (paginated)
- **GET** `api/v1/employees/{id}` - Get employee by ID
- **GET** `api/v1/employees/number/{employeeNumber}` - Get employee by employee number
- **POST** `api/v1/employees` - Create new employee
- **PUT** `api/v1/employees/{id}` - Update employee
- **DELETE** `api/v1/employees/{id}` - Delete employee
- **POST** `api/v1/employees/{id}/terminate` - Terminate employee

**Query Parameters:**
- `companyId` (required)
- `page` (default: 1)
- `pageSize` (default: 25)
- `search` (optional)

**Authorization:** Required

---

## ⏰ Timesheets

### Timesheet Management
- **GET** `api/v1/timesheets` - List all timesheets (paginated)
- **GET** `api/v1/timesheets/{id}` - Get timesheet by ID
- **GET** `api/v1/timesheets/employee/{employeeId}` - Get timesheets for employee
- **POST** `api/v1/timesheets` - Create new timesheet
- **PUT** `api/v1/timesheets/{id}` - Update timesheet
- **DELETE** `api/v1/timesheets/{id}` - Delete timesheet

### Timesheet Actions
- **POST** `api/v1/timesheets/{id}/submit` - Submit timesheet for approval
- **POST** `api/v1/timesheets/{id}/approve` - Approve timesheet
- **POST** `api/v1/timesheets/{id}/reject` - Reject timesheet

**Query Parameters:**
- `companyId` (required)
- `startDate` (optional, ISO 8601)
- `endDate` (optional, ISO 8601)
- `employeeId` (optional)
- `status` (optional: Draft, Submitted, Approved, Rejected)

**Authorization:** Required

---

## 💰 Payroll

### Payroll Periods
- **POST** `api/v1/payroll/periods` - Create payroll period
- **GET** `api/v1/payroll/periods/{id}` - Get payroll period by ID
- **GET** `api/v1/payroll/periods/year/{year}` - Get payroll periods by year
- **POST** `api/v1/payroll/periods/{id}/process` - Process payroll period
- **POST** `api/v1/payroll/periods/{id}/approve` - Approve payroll period
- **GET** `api/v1/payroll/periods/{id}/report` - Get payroll period report

### Payroll Records
- **GET** `api/v1/payroll/records/{id}` - Get payroll record by ID
- **GET** `api/v1/payroll/records/employee/{employeeId}` - Get payroll records for employee
- **POST** `api/v1/payroll/records/{id}/recalculate` - Recalculate payroll record
- **GET** `api/v1/payroll/records/{id}/paystub` - Get paystub for payroll record

**Query Parameters:**
- `companyId` (required)
- `year` (for year-based queries)

**Authorization:** Required

---

## 📋 Compliance

### Compliance Management
- **GET** `api/v1/compliance/score` - Get overall compliance score
- **GET** `api/v1/compliance/violations/active` - Get active violations
- **GET** `api/v1/compliance/violations/{id}` - Get violation by ID
- **POST** `api/v1/compliance/violations/{id}/resolve` - Resolve violation
- **GET** `api/v1/compliance/report` - Get compliance report

**Query Parameters:**
- `companyId` (required)
- `count` (for recent violations, default: 10)

**Authorization:** Required

---

## 💼 Finance Module

### Chart of Accounts
- **GET** `api/v1/finance/accounts` - List all accounts (paginated)
- **GET** `api/v1/finance/accounts/{id}` - Get account by ID
- **POST** `api/v1/finance/accounts` - Create new account
- **PUT** `api/v1/finance/accounts/{id}` - Update account
- **DELETE** `api/v1/finance/accounts/{id}` - Delete account

**Query Parameters:**
- `companyId` (required)
- `page` (default: 1)
- `pageSize` (default: 25)
- `search` (optional)
- `type` (optional: Asset, Liability, Equity, Revenue, Expense)

**Authorization:** Required

---

### Account Templates (FASB Compliance)
- **GET** `api/v1/finance/fasb-topics` - Get FASB topics
- **GET** `api/v1/finance/fasb-topics/{topicId}/subtopics` - Get FASB subtopics
- **GET** `api/v1/finance/account-templates` - Get account templates
- **POST** `api/v1/finance/account-templates/{code}/load` - Load account template

**Query Parameters:**
- `companyId` (required for templates)

**Authorization:** Required

---

### Journal Entries
- **GET** `api/v1/finance/journal-entries` - List all journal entries (paginated)
- **GET** `api/v1/finance/journal-entries/{id}` - Get journal entry by ID
- **POST** `api/v1/finance/journal-entries` - Create new journal entry
- **POST** `api/v1/finance/journal-entries/{id}/post` - Post journal entry to ledger
- **POST** `api/v1/finance/journal-entries/{id}/void` - Void journal entry
- **DELETE** `api/v1/finance/journal-entries/{id}` - Delete draft journal entry

**Query Parameters:**
- `companyId` (required)
- `page` (default: 1)
- `pageSize` (default: 25)
- `search` (optional)
- `status` (optional: Draft, Posted, Void)

**Authorization:** Required

---

### Vendors
- **GET** `api/v1/vendors` - List all vendors (paginated)
- **GET** `api/v1/vendors/{id}` - Get vendor by ID
- **GET** `api/v1/vendors/{id}/balance` - Get vendor balance
- **POST** `api/v1/vendors` - Create new vendor
- **PUT** `api/v1/vendors/{id}` - Update vendor
- **DELETE** `api/v1/vendors/{id}` - Delete vendor

**Query Parameters:**
- `companyId` (required)
- `page` (default: 1)
- `pageSize` (default: 25)
- `search` (optional)
- `activeOnly` (optional, boolean)

**Authorization:** Required

---

### Bills (Accounts Payable)
- **GET** `api/v1/vendors/bills/{id}` - Get bill by ID
- **GET** `api/v1/vendors/bills/vendor/{vendorId}` - Get bills for vendor
- **POST** `api/v1/vendors/bills` - Create new bill

**Query Parameters:**
- `companyId` (required)
- `page` (default: 1)
- `pageSize` (default: 25)
- `status` (optional: Unpaid, Paid, Void)
- `search` (optional)

**Authorization:** Required

**Note:** Bills endpoint is nested under vendors (`/vendors/bills/`) not `/finance/bills/`

---

## 📝 RESTful Conventions

All endpoints follow standard HTTP verbs:

```
GET    /api/v1/{module}/{resource}           → List all (paginated)
GET    /api/v1/{module}/{resource}/{id}      → Get one by ID
POST   /api/v1/{module}/{resource}           → Create new
PUT    /api/v1/{module}/{resource}/{id}      → Update existing
DELETE /api/v1/{module}/{resource}/{id}      → Delete

POST   /api/v1/{module}/{resource}/{id}/{action} → Custom action
```

---

## 🔍 Query Parameters Standard

### Pagination
- `page` (default: 1) - Page number
- `pageSize` (default: 25, max: 100) - Items per page

### Filtering
- `status` - Filter by status (varies by resource)
- `search` - Search across relevant fields
- `activeOnly` - Boolean flag for active records only

### Multi-tenant
- `companyId` - **REQUIRED** for all company-scoped endpoints

### Date Ranges
- `startDate` - ISO 8601 date string (e.g., "2024-01-01")
- `endDate` - ISO 8601 date string (e.g., "2024-12-31")

---

## 🚨 Common Errors

### 401 Unauthorized
Missing or invalid authentication token

### 403 Forbidden
User doesn't have permission for the requested resource

### 404 Not Found
- Resource ID doesn't exist
- **Wrong endpoint path** (most common)

### 400 Bad Request
- Missing required parameters
- Invalid data format
- Validation errors

---

## 🎯 Best Practices

1. **Always include `companyId`** in query parameters for multi-tenant isolation
2. **Use proper HTTP verbs** - GET for retrieval, POST for creation/actions, PUT for updates, DELETE for removal
3. **Include Authorization header** - `Authorization: Bearer {token}`
4. **Handle pagination** - Always use `page` and `pageSize` for list endpoints
5. **Check status codes** - Don't assume 200 OK, handle errors properly
6. **Use `/v1/` prefix** - All endpoints must include API version

---

## 📦 Response Format

### Success Response (200 OK)
```json
{
  "data": { /* resource or array */ },
  "success": true
}
```

### Paginated Response
```json
{
  "items": [ /* array of resources */ ],
  "page": 1,
  "pageSize": 25,
  "totalCount": 150,
  "totalPages": 6
}
```

### Error Response (4xx, 5xx)
```json
{
  "error": "Error message",
  "details": { /* additional error details */ },
  "success": false
}
```

---

## 🛡️ Security Notes

- All endpoints (except `/auth/login`) require authentication
- JWT tokens expire after configurable timeout
- Use `/auth/refresh` to renew tokens
- HTTPS required in production
- Rate limiting may apply

---

**END OF DOCUMENT**
