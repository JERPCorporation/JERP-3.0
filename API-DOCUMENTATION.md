# JERP 3.0 API Documentation

## Overview

The JERP 3.0 API is a comprehensive Enterprise Resource Planning system with payroll, finance, and compliance features.

**Base URL:** `http://localhost:5000`  
**API Version:** 3.0.0  
**Last Updated:** February 2026

## Security & Authentication

### Authentication Overview

JERP 3.0 uses **JWT (JSON Web Token)** authentication for secure API access. All endpoints except `/api/v1/auth/login` and `/api/v1/auth/register` require authentication.

### Authentication Flow

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant Database
    
    Client->>API: POST /api/v1/auth/login (credentials)
    API->>Database: Validate credentials
    Database-->>API: User data
    API-->>Client: JWT token + refresh token
    
    Client->>API: GET /api/v1/employees (with JWT)
    API->>API: Validate JWT signature & expiration
    API->>Database: Query data (with tenant filter)
    Database-->>API: Employee data
    API-->>Client: Response data
```

### JWT Token Details

**Token Structure:**
```json
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "sub": "user-id",
    "email": "user@example.com",
    "role": "Admin,FinanceManager",
    "companyId": "company-guid",
    "exp": 1234567890,
    "iat": 1234567890
  }
}
```

**Token Properties:**
- **Algorithm:** HS256 (HMAC-SHA256)
- **Expiration:** 1 hour (3600 seconds)
- **Claims:** User ID, email, roles, company ID (tenant)
- **Refresh Token:** 7 days (can be used to get new access token)

**Security Features:**
- ✅ Signature verification with secret key
- ✅ Expiration validation
- ✅ Multi-tenant isolation via CompanyId claim
- ✅ Role-based authorization
- ⏳ Token refresh mechanism (Phase 2.5)
- ⏳ Token revocation (Phase 2.5)

### Login Endpoint

**Request:**
```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}
```

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh-token-string",
    "expiresIn": 3600,
    "tokenType": "Bearer",
    "user": {
      "id": "user-guid",
      "email": "admin@example.com",
      "firstName": "Admin",
      "lastName": "User",
      "roles": ["Admin", "FinanceManager"]
    }
  }
}
```

**Response (Failed - 401 Unauthorized):**
```json
{
  "success": false,
  "error": "Invalid username or password",
  "code": "INVALID_CREDENTIALS"
}
```

### Using the Token

Include the JWT token in the `Authorization` header for all authenticated requests:

```http
GET /api/v1/employees
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Authorization Header Format:**
```
Authorization: Bearer <token>
```

### Role-Based Access Control (RBAC)

**Available Roles:**
- `SystemAdmin` - Full system access
- `CompanyAdmin` - Company-level administration
- `FinanceManager` - Finance module access
- `Accountant` - Accounting operations
- `InventoryManager` - Inventory management
- `SalesManager` - Sales operations
- `Auditor` - Read-only access

**Permission Examples:**
- Creating journal entries requires `FinanceManager` or `Accountant` role
- Posting journal entries requires `FinanceManager` role only
- Viewing financial reports requires `FinanceManager`, `Accountant`, or `Auditor` role

### Security Headers

**Planned Security Headers (Phase 2.5):**
- `Strict-Transport-Security: max-age=31536000; includeSubDomains`
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Content-Security-Policy: default-src 'self'`
- `Referrer-Policy: strict-origin-when-cross-origin`

### Rate Limiting

**Planned Rate Limiting Policies (Phase 2.5):**

| Endpoint Type | Rate Limit | Window |
|--------------|------------|--------|
| Authentication | 5 requests | 15 minutes |
| General API | 100 requests | 1 minute |
| Reporting | 20 requests | 1 minute |
| File Upload | 10 requests | 1 minute |

**Rate Limit Headers:**
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1234567890
```

**Rate Limit Exceeded Response (429 Too Many Requests):**
```json
{
  "success": false,
  "error": "Rate limit exceeded. Please try again later.",
  "code": "RATE_LIMIT_EXCEEDED",
  "retryAfter": 60
}
```

---

## API Endpoints

### Employees
- `GET /api/v1/employees` - List all employees
- `GET /api/v1/employees/{id}` - Get employee by ID
- `POST /api/v1/employees` - Create new employee
- `PUT /api/v1/employees/{id}` - Update employee
- `DELETE /api/v1/employees/{id}` - Delete employee
- `POST /api/v1/employees/{id}/terminate` - Terminate employee

### Timesheets
- `GET /api/v1/timesheets/employee/{employeeId}` - Get timesheets for employee
- `GET /api/v1/timesheets/{id}` - Get timesheet by ID
- `POST /api/v1/timesheets` - Create timesheet
- `PUT /api/v1/timesheets/{id}` - Update timesheet
- `POST /api/v1/timesheets/{id}/submit` - Submit for approval
- `POST /api/v1/timesheets/{id}/approve` - Approve timesheet
- `POST /api/v1/timesheets/{id}/reject` - Reject timesheet

### Payroll
- `GET /api/v1/payroll/periods` - List pay periods
- `GET /api/v1/payroll/periods/{id}` - Get pay period details
- `POST /api/v1/payroll/periods` - Create pay period
- `POST /api/v1/payroll/periods/{id}/process` - Process payroll
- `POST /api/v1/payroll/periods/{id}/approve` - Approve payroll
- `GET /api/v1/payroll/records/{id}` - Get payroll record
- `GET /api/v1/payroll/records/employee/{employeeId}` - Get employee payroll records

### Departments
- `GET /api/v1/departments` - List all departments
- `GET /api/v1/departments/{id}` - Get department by ID
- `POST /api/v1/departments` - Create department
- `PUT /api/v1/departments/{id}` - Update department
- `DELETE /api/v1/departments/{id}` - Delete department

### Reports
- `GET /api/v1/reports/payroll/summary` - Payroll summary report
- `GET /api/v1/reports/employees/hours` - Employee hours report
- `GET /api/v1/reports/compliance/violations` - Compliance violations
- `GET /api/v1/reports/departments/summary` - Department summary

### Dashboard
- `GET /api/v1/dashboard/overview` - Dashboard overview statistics
- `GET /api/v1/dashboard/payroll-metrics` - Payroll metrics
- `GET /api/v1/dashboard/compliance-trend` - Compliance trend
- `GET /api/v1/dashboard/employee-distribution` - Employee distribution
- `GET /api/v1/dashboard/pending-approvals` - Pending approvals

### Compliance
- `GET /api/v1/compliance/violations` - List violations
- `GET /api/v1/compliance/score` - Get compliance score

## Error Responses

```json
{
  "success": false,
  "error": "Error message here"
}
```

## Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `429 Too Many Requests` - Rate limit exceeded (Phase 2.5)
- `500 Internal Server Error` - Server error

## Security Considerations

### Input Validation

All API endpoints use **FluentValidation** for request validation:

**Example Validation Rules:**
- Email addresses must be valid format
- Required fields cannot be null or empty
- Numeric values must be within acceptable ranges
- Dates must be in valid ISO 8601 format

**Validation Error Response (400 Bad Request):**
```json
{
  "success": false,
  "error": "Validation failed",
  "errors": {
    "Email": ["Email address is invalid"],
    "Password": ["Password must be at least 8 characters"]
  }
}
```

### SQL Injection Prevention

All database queries use **Entity Framework Core** with parameterized queries:
- ✅ No raw SQL queries (unless absolutely necessary)
- ✅ Automatic parameter escaping
- ✅ Type-safe LINQ queries

### Cross-Site Scripting (XSS) Prevention

- ✅ React automatically escapes output
- ✅ API returns JSON (not HTML)
- ✅ Content-Type headers properly set
- ⏳ Content-Security-Policy header (Phase 2.5)

### CORS Configuration

**Allowed Origins (Production):**
```
https://app.jerp.io
https://admin.jerp.io
https://partner.jerp.io
```

**Allowed Origins (Development):**
```
http://localhost:3000
http://localhost:3001
```

**CORS Headers:**
- `Access-Control-Allow-Origin`
- `Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS`
- `Access-Control-Allow-Headers: Content-Type, Authorization`
- `Access-Control-Allow-Credentials: true`

### Multi-Tenant Isolation

**Automatic Tenant Filtering:**
- Every API request includes `CompanyId` from JWT token
- All database queries automatically filtered by tenant
- Prevents cross-tenant data access
- No manual tenant checks required in business logic

**Example:**
```csharp
// Automatic query filter applied
var accounts = await _context.Accounts
    .Where(a => a.Type == AccountType.Asset)
    .ToListAsync();
// Only returns accounts for current tenant (CompanyId from JWT)
```

### Audit Logging

All sensitive operations are logged:
- ✅ User authentication attempts (success/failure)
- ✅ Journal entry creation, posting, voiding
- ✅ Payment processing
- ✅ User management operations
- ⏳ Enhanced security event logging (Phase 2.5)

**Audit Log Structure:**
```json
{
  "timestamp": "2026-02-07T12:00:00Z",
  "userId": "user-guid",
  "companyId": "company-guid",
  "action": "JournalEntry.Posted",
  "entityId": "entry-guid",
  "ipAddress": "192.168.1.100",
  "userAgent": "Mozilla/5.0..."
}
```

### Data Encryption

**In Transit:**
- ✅ HTTPS/TLS 1.2+ required in production
- ✅ Certificate from Let's Encrypt or commercial CA

**At Rest:**
- ⏳ SQL Server Transparent Data Encryption (TDE) recommended
- ⏳ Encrypted database backups (Phase 2.5)

### Security Best Practices for API Consumers

1. **Store JWT tokens securely**
   - Use httpOnly cookies or secure storage
   - Never store in localStorage (XSS vulnerable)
   - Clear tokens on logout

2. **Implement token refresh**
   - Request new token before expiration
   - Handle 401 responses gracefully

3. **Use HTTPS only**
   - Never send credentials over HTTP
   - Validate SSL certificates

4. **Handle errors gracefully**
   - Don't expose sensitive error details to users
   - Log errors for debugging

5. **Implement rate limiting on client side**
   - Cache responses when appropriate
   - Batch requests when possible

---

## Interactive Documentation

Visit `http://localhost:5000/swagger` for Swagger UI with interactive API documentation.

**Swagger Features:**
- Interactive API testing
- Request/response examples
- Schema documentation
- Authentication testing
- Try out endpoints directly from browser
