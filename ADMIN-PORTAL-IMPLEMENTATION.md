# Admin Portal Core - Phase 1: Audit Logging System

## Overview
This implementation provides a foundational audit logging system with blockchain-style hash chaining for immutability and tamper detection.

## Features Implemented

### 1. Hash-Chained Audit Log
- **Blockchain-Style Chain**: Each audit log entry contains a hash of the previous entry, forming an immutable chain
- **SHA256 Hashing**: Uses cryptographic SHA256 hash (first 12 characters per spec)
- **Genesis Block**: First entry per company starts with PreviousHash = "GENESIS"
- **Sequence Numbers**: Unique sequential numbers per company for ordering
- **Tamper Detection**: Any modification breaks the hash chain and can be detected

### 2. Audit Log Entity (`AuditLog`)
**Location**: `src/JERP.Core/Entities/AuditLog.cs`

**Key Fields**:
- `CompanyId`: Multi-tenant isolation
- `UserId`, `UserEmail`, `UserName`: User who performed the action
- `Action`: Type of action (e.g., "payroll_processed", "employee_created")
- `Resource`: Resource identifier (e.g., "PayPeriod:123", "Employee:456")
- `Details`: Human-readable description
- `SequenceNumber`: Sequential ordering within company
- `PreviousHash`: Hash of previous entry (or "GENESIS" for first)
- `CurrentHash`: Hash of this entry
- `Timestamp`: UTC timestamp
- `IpAddress`: IP address of the user

### 3. User Entity Enhancements
**Location**: `src/JERP.Core/Entities/User.cs`

**New Enums**:
- `UserRole`: SuperAdmin, Admin, PayrollManager, HRManager, Accountant, Employee
- `UserStatus`: Active, Suspended, Inactive

**New Properties**:
- `Role`: User's role in the system
- `Status`: Current account status
- `LastLoginIp`: IP address of last login
- `FailedLoginAttempts`: Track failed login attempts
- `LockoutUntil`: Account lockout timestamp for security

### 4. Audit Log Service
**Location**: `src/JERP.Application/Services/Security/AuditLogService.cs`

**Methods**:
- `LogAction()`: Creates new audit log entry with hash chain
- `GetAuditLog()`: Queries audit logs with filtering and pagination
- `VerifyAuditChain()`: Verifies integrity of entire chain
- `ExportToCsv()`: Exports audit logs to CSV format

**Hash Calculation**:
```
Data = CompanyId|Timestamp|UserId|Action|Resource|Details|IpAddress|PreviousHash|SequenceNumber
Hash = SHA256(Data).Substring(0, 12)
```

**Chain Logic**:
1. First entry: `PreviousHash = "GENESIS"`
2. Subsequent entries: `PreviousHash = previous entry's CurrentHash`
3. Verification: Recalculate all hashes and verify chain links

### 5. Admin API Endpoints
**Location**: `src/JERP.Api/Controllers/AdminController.cs`

**Endpoints**:

#### GET /api/v1/admin/audit-log
Query audit logs with filtering and pagination.

**Query Parameters**:
- `companyId` (required): Company ID
- `startDate` (optional): Filter by start date
- `endDate` (optional): Filter by end date
- `action` (optional): Filter by action type
- `userId` (optional): Filter by user
- `page` (default: 1): Page number
- `pageSize` (default: 50, max: 100): Results per page

**Response**:
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    "logs": [
      {
        "id": "guid",
        "companyId": "guid",
        "sequenceNumber": 1,
        "timestamp": "2026-02-04T08:30:00Z",
        "userId": "guid",
        "userEmail": "ichbincesartobar@yahoo.com",
        "userName": "Julio Mendez",
        "action": "payroll_processed",
        "resource": "PayPeriod:123",
        "details": "Processed 25 employee records...",
        "ipAddress": "192.168.1.1",
        "previousHash": "GENESIS",
        "currentHash": "A1B2C3D4E5F6"
      }
    ],
    "pagination": {
      "total": 100,
      "page": 1,
      "pageSize": 50,
      "totalPages": 2
    }
  }
}
```

#### POST /api/v1/admin/audit-log/verify
Verify the integrity of the audit chain.

**Request Body**:
```json
{
  "companyId": "guid"
}
```

**Response (Valid Chain)**:
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    "isValid": true,
    "message": "Chain verified successfully. All 150 entries are valid.",
    "brokenLinks": [],
    "timestamp": "2026-02-04T08:30:00Z"
  }
}
```

**Response (Broken Chain)**:
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    "isValid": false,
    "message": "Chain verification failed with 2 broken link(s).",
    "brokenLinks": [
      "Entry Seq 42: Hash mismatch. Expected ABC123, got XYZ789",
      "Entry Seq 43: Chain broken. PreviousHash 'XYZ789' does not match previous entry's CurrentHash 'DEF456'"
    ],
    "timestamp": "2026-02-04T08:30:00Z"
  }
}
```

#### GET /api/v1/admin/audit-log/export
Export audit logs to CSV format.

**Query Parameters**:
- `companyId` (required): Company ID
- `startDate` (optional): Filter by start date
- `endDate` (optional): Filter by end date

**Response**: CSV file download
```csv
SequenceNumber,Timestamp,UserId,UserEmail,UserName,Action,Resource,Details,IpAddress,PreviousHash,CurrentHash
1,"2026-02-04 08:00:00",guid,"ichbincesartobar@yahoo.com","Julio Mendez","employee_created","Employee:123","Created employee John Doe (#EMP001)","192.168.1.1","GENESIS","A1B2C3D4E5F6"
2,"2026-02-04 08:05:00",guid,"ichbincesartobar@yahoo.com","Julio Mendez","payroll_processed","PayPeriod:456","Processed 25 employee records. Gross: $50,000.00","192.168.1.1","A1B2C3D4E5F6","B2C3D4E5F6A7"
```

### 6. Automatic Audit Logging
Audit logging is automatically integrated into critical operations:

#### PayrollService
**Actions Logged**:
- `payroll_processed`: When payroll is calculated for a pay period
- `payroll_approved`: When payroll is approved for payment

#### EmployeeService  
**Actions Logged**:
- `employee_created`: When a new employee is added
- `employee_updated`: When employee information is modified

#### PayrollToFinanceService
**Actions Logged**:
- `gl_entry_posted`: When payroll transactions are posted to general ledger

### 7. Database Migration
**Location**: `src/JERP.Infrastructure/Data/Migrations/20260204082239_AddAuditLogSystem.cs`

**Changes**:
- Added `CompanyId`, `UserEmail`, `UserName`, `Resource`, `Details`, `SequenceNumber` to AuditLogs
- Added `Role`, `Status`, `LastLoginIp`, `FailedLoginAttempts`, `LockoutUntil` to Users
- Created indexes:
  - `IX_AuditLogs_CompanyId`
  - `IX_AuditLogs_CompanyId_Timestamp`
  - `IX_AuditLogs_CompanyId_SequenceNumber` (unique)

**To Apply**:
```bash
dotnet ef database update --project src/JERP.Infrastructure --startup-project src/JERP.Api
```

## Security Considerations

### Hash Length
- **Current**: 12 characters (48-bit space, ~281 trillion combinations)
- **Trade-off**: Per requirements vs collision resistance
- **Risk**: Lower collision resistance than full hash
- **Mitigation**: Acceptable for most use cases; documented for review

### User Context
- **Current**: Placeholder values (Guid.Empty, "ichbincesartobar@yahoo.com")
- **Required**: Implement proper HttpContext user retrieval before production
- **Impact**: Cannot identify actual users in current state
- **TODO**: Added comments throughout codebase

### Authorization
- **Current**: Requires authentication only
- **Required**: Add role-based authorization to AdminController
- **Recommendation**: Use `[Authorize(Roles = "Admin,SuperAdmin")]` or policy-based authorization
- **TODO**: Added comment in AdminController

### Distributed Locking
- **Current**: In-memory lock for sequence number generation
- **Limitation**: Only works for single-instance deployments
- **Required for Scale**: Distributed locking (e.g., Redis) or optimistic concurrency
- **TODO**: Added comment in AuditLogService

### Audit Logging Failures
- **Strategy**: All audit logging wrapped in try-catch
- **Behavior**: Failures are logged but don't break business operations
- **Trade-off**: Ensures reliability but may miss audit entries on errors

## Testing

### Manual Testing Workflow

1. **Create Audit Log Entries**:
   ```bash
   # Create employee
   POST /api/v1/employees
   # Process payroll
   POST /api/v1/payroll/{payPeriodId}/process
   # Approve payroll
   POST /api/v1/payroll/{payPeriodId}/approve
   ```

2. **Query Audit Logs**:
   ```bash
   GET /api/v1/admin/audit-log?companyId={guid}&page=1&pageSize=50
   ```

3. **Verify Chain Integrity**:
   ```bash
   POST /api/v1/admin/audit-log/verify
   Body: { "companyId": "{guid}" }
   ```

4. **Test Tampering Detection**:
   ```sql
   -- Manually modify an audit log entry in database
   UPDATE "AuditLogs" SET "Details" = 'Modified!' WHERE "SequenceNumber" = 5;
   ```
   ```bash
   # Verify chain - should detect broken link
   POST /api/v1/admin/audit-log/verify
   Body: { "companyId": "{guid}" }
   ```

5. **Export to CSV**:
   ```bash
   GET /api/v1/admin/audit-log/export?companyId={guid}
   ```

### Expected Behavior

✅ **Valid Chain**:
- All hashes recalculate correctly
- Each entry's PreviousHash matches previous entry's CurrentHash
- Verification returns `isValid: true`

❌ **Broken Chain**:
- Modified entries fail hash recalculation
- Chain links are broken
- Verification returns `isValid: false` with detailed error messages

## Security Scan Results

✅ **CodeQL Analysis**: 0 vulnerabilities detected

## Production Readiness Checklist

Before deploying to production:

- [ ] Implement proper user context retrieval (userId, userEmail, userName, ipAddress)
- [ ] Add role-based authorization to AdminController
- [ ] Consider distributed locking for multi-instance deployments
- [ ] Review hash length requirements vs security needs
- [ ] Implement account lockout logic based on FailedLoginAttempts
- [ ] Set up monitoring and alerting for audit chain verification failures
- [ ] Test multi-tenant isolation (separate chains per company)
- [ ] Load test audit logging performance
- [ ] Set up regular automated chain verification

## Future Enhancements

- **Real-time Alerts**: Notify admins when chain verification fails
- **Advanced Filtering**: Add more filter options to audit log queries
- **Audit Retention**: Implement automatic archival of old audit logs
- **Compliance Reports**: Generate compliance-specific audit reports
- **User Activity Dashboard**: Visualize user actions and patterns
- **Audit Log Replication**: Replicate audit logs to separate secure storage
- **Digital Signatures**: Add digital signatures for non-repudiation
