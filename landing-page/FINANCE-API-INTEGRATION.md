# Finance Module API Integration Layer

## Overview

This document provides comprehensive documentation for the Finance Module API integration layer. The integration connects the Finance Module UI with the backend API controllers, replacing mock data with real API calls.

## Architecture

### Directory Structure

```
landing-page/
├── services/api/          # API service layer
│   ├── apiClient.ts       # Axios client with interceptors
│   ├── accountsApi.ts     # Chart of Accounts API
│   ├── journalEntriesApi.ts
│   ├── generalLedgerApi.ts
│   ├── vendorsApi.ts
│   ├── billsApi.ts
│   └── reportsApi.ts
├── hooks/                 # React Query hooks
│   ├── useAccounts.ts
│   ├── useJournalEntries.ts
│   ├── useVendors.ts
│   └── useFinancialReports.ts
├── types/
│   └── finance.ts         # TypeScript type definitions
└── lib/
    └── queryClient.ts     # React Query configuration
```

## Configuration

### Environment Variables

Create a `.env.local` file in the `landing-page` directory:

```bash
VITE_API_URL=https://localhost:7001
VITE_AUTH_TOKEN_KEY=authToken
```

**Available environments:**
- `.env.development` - Local development (https://localhost:7001)
- `.env.production` - Production (https://api.jerp.io)

### API Client

The API client (`services/api/apiClient.ts`) is configured with:

- **Base URL**: Reads from environment variables
- **Timeout**: 30 seconds
- **Auth**: Automatically adds Bearer token from localStorage
- **Error Handling**: Redirects to login on 401 errors

## Usage Examples

### 1. Chart of Accounts

```typescript
'use client';

import { useAccounts } from '@/hooks/useAccounts';

export default function ChartOfAccountsPage() {
  const companyId = 'your-company-id'; // Get from auth context
  const { accounts, isLoading, error, createAccount, isCreating } = useAccounts(companyId);

  if (isLoading) {
    return <div>Loading accounts...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  const handleCreateAccount = () => {
    createAccount({
      companyId,
      accountNumber: '1000',
      accountName: 'Cash',
      type: 'Asset',
      isCOGS: false,
    });
  };

  return (
    <div>
      <h1>Chart of Accounts</h1>
      <button onClick={handleCreateAccount} disabled={isCreating}>
        {isCreating ? 'Creating...' : 'Add Account'}
      </button>
      <table>
        <thead>
          <tr>
            <th>Account #</th>
            <th>Name</th>
            <th>Type</th>
            <th>Balance</th>
          </tr>
        </thead>
        <tbody>
          {accounts.map((account) => (
            <tr key={account.id}>
              <td>{account.accountNumber}</td>
              <td>{account.accountName}</td>
              <td>{account.type}</td>
              <td>${account.balance.toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

### 2. Journal Entries

```typescript
'use client';

import { useJournalEntries } from '@/hooks/useJournalEntries';

export default function JournalEntriesPage() {
  const companyId = 'your-company-id';
  const { 
    journalEntries, 
    isLoading, 
    createJournalEntry, 
    postJournalEntry 
  } = useJournalEntries(companyId);

  const handleCreate = () => {
    createJournalEntry({
      companyId,
      transactionDate: '2026-02-05',
      description: 'Monthly depreciation',
      entries: [
        {
          accountId: 'expense-account-id',
          debitAmount: 1000,
          creditAmount: 0,
          description: 'Depreciation expense',
        },
        {
          accountId: 'contra-asset-id',
          debitAmount: 0,
          creditAmount: 1000,
          description: 'Accumulated depreciation',
        },
      ],
    });
  };

  const handlePost = (id: string) => {
    postJournalEntry(id);
  };

  return (
    <div>
      <h1>Journal Entries</h1>
      <button onClick={handleCreate}>Create Entry</button>
      {isLoading ? (
        <div>Loading...</div>
      ) : (
        <ul>
          {journalEntries.map((entry) => (
            <li key={entry.id}>
              {entry.journalEntryNumber} - {entry.description}
              {entry.status === 'Draft' && (
                <button onClick={() => handlePost(entry.id)}>Post</button>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
```

### 3. Vendors (Accounts Payable)

```typescript
'use client';

import { useVendors } from '@/hooks/useVendors';

export default function VendorsPage() {
  const companyId = 'your-company-id';
  const { 
    vendors, 
    isLoading, 
    createVendor, 
    updateVendor, 
    deleteVendor 
  } = useVendors(companyId);

  const handleCreate = () => {
    createVendor({
      companyName: 'ACME Corp',
      contactPerson: 'John Doe',
      email: 'john@acme.com',
      phone: '555-0100',
      address: '123 Main St',
    });
  };

  const handleUpdate = (id: string) => {
    updateVendor({
      id,
      data: {
        companyName: 'ACME Corporation',
        isActive: true,
      },
    });
  };

  return (
    <div>
      <h1>Vendors</h1>
      <button onClick={handleCreate}>Add Vendor</button>
      {isLoading ? (
        <div>Loading...</div>
      ) : (
        <table>
          <tbody>
            {vendors.map((vendor) => (
              <tr key={vendor.id}>
                <td>{vendor.companyName}</td>
                <td>${vendor.balance.toLocaleString()}</td>
                <td>
                  <button onClick={() => handleUpdate(vendor.id)}>Edit</button>
                  <button onClick={() => deleteVendor(vendor.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
```

### 4. Financial Reports

```typescript
'use client';

import { useState } from 'react';
import { useFinancialReports } from '@/hooks/useFinancialReports';

export default function ReportsPage() {
  const companyId = 'your-company-id';
  const [dateRange, setDateRange] = useState({
    startDate: '2026-01-01',
    endDate: '2026-01-31',
  });

  const { profitAndLoss, balanceSheet, isLoading } = useFinancialReports({
    companyId,
    startDate: dateRange.startDate,
    endDate: dateRange.endDate,
  });

  if (isLoading) {
    return <div>Generating reports...</div>;
  }

  return (
    <div>
      <h1>Financial Reports</h1>
      
      {/* Date Range Selector */}
      <div>
        <input
          type="date"
          value={dateRange.startDate}
          onChange={(e) => setDateRange({ ...dateRange, startDate: e.target.value })}
        />
        <input
          type="date"
          value={dateRange.endDate}
          onChange={(e) => setDateRange({ ...dateRange, endDate: e.target.value })}
        />
      </div>

      {/* Profit & Loss */}
      {profitAndLoss && (
        <div>
          <h2>Profit & Loss Statement</h2>
          <p>Total Revenue: ${profitAndLoss.totalRevenue.toLocaleString()}</p>
          <p>Total Expenses: ${profitAndLoss.totalExpenses.toLocaleString()}</p>
          <p>Net Income: ${profitAndLoss.netIncome.toLocaleString()}</p>
          <p>Profit Margin: {profitAndLoss.profitMargin.toFixed(2)}%</p>
        </div>
      )}

      {/* Balance Sheet */}
      {balanceSheet && (
        <div>
          <h2>Balance Sheet</h2>
          <p>Total Assets: ${balanceSheet.assets.totalAssets.toLocaleString()}</p>
          <p>Total Liabilities: ${balanceSheet.liabilities.totalLiabilities.toLocaleString()}</p>
          <p>Total Equity: ${balanceSheet.equity.totalEquity.toLocaleString()}</p>
        </div>
      )}
    </div>
  );
}
```

## API Service Methods

### accountsApi

```typescript
// Get all accounts
accountsApi.getAll(companyId: string): Promise<Account[]>

// Get account by ID
accountsApi.getById(id: string): Promise<Account>

// Create account
accountsApi.create(data: CreateAccountRequest): Promise<Account>

// Update account
accountsApi.update(id: string, data: UpdateAccountRequest): Promise<Account>

// Get FASB topics
accountsApi.getFASBTopics(category?: string): Promise<any[]>

// Get FASB subtopics
accountsApi.getFASBSubtopics(topicId: string): Promise<any[]>
```

### journalEntriesApi

```typescript
// Get all journal entries
journalEntriesApi.getAll(
  companyId: string,
  startDate?: string,
  endDate?: string,
  status?: JournalEntryStatus
): Promise<JournalEntry[]>

// Get by ID
journalEntriesApi.getById(id: string): Promise<JournalEntry>

// Create
journalEntriesApi.create(data: CreateJournalEntryRequest): Promise<JournalEntry>

// Post entry
journalEntriesApi.post(id: string): Promise<{ message: string }>

// Void entry
journalEntriesApi.void(id: string): Promise<{ message: string }>
```

### vendorsApi

```typescript
// Get all vendors
vendorsApi.getAll(companyId: string, isActive?: boolean): Promise<Vendor[]>

// Get by ID
vendorsApi.getById(id: string): Promise<Vendor>

// Create
vendorsApi.create(companyId: string, data: CreateVendorDto): Promise<Vendor>

// Update
vendorsApi.update(id: string, data: UpdateVendorDto): Promise<Vendor>

// Delete
vendorsApi.delete(id: string): Promise<void>

// Get balance
vendorsApi.getBalance(id: string): Promise<{ vendorId: string; balance: number }>
```

### reportsApi

```typescript
// Profit & Loss
reportsApi.getProfitAndLoss(request: ReportRequest): Promise<ProfitAndLossReport>

// Balance Sheet
reportsApi.getBalanceSheet(request: ReportRequest): Promise<BalanceSheetReport>

// Cash Flow
reportsApi.getCashFlow(request: ReportRequest): Promise<any>

// Export P&L to PDF
reportsApi.exportProfitAndLossPdf(request: ReportRequest): Promise<Blob>

// Export Balance Sheet to Excel
reportsApi.exportBalanceSheetExcel(request: ReportRequest): Promise<Blob>
```

## React Query Hooks

All hooks return consistent interfaces:

```typescript
{
  data: T | T[],           // The fetched data
  isLoading: boolean,      // Loading state
  error: Error | null,     // Error object
  mutate: Function,        // For mutations (create/update/delete)
  isPending: boolean,      // Mutation loading state
}
```

### Query Configuration

Queries are configured with:

- **Retry**: 3 attempts with exponential backoff
- **Stale Time**: 5 minutes
- **Refetch on Window Focus**: Disabled

### Cache Invalidation

Mutations automatically invalidate related queries:

```typescript
// After creating an account, the accounts list is refetched
createAccount(data) // → invalidates ['accounts', companyId]

// After posting a journal entry, all entries are refetched
postJournalEntry(id) // → invalidates ['journalEntries', companyId]
```

## Error Handling

### Toast Notifications

Success and error messages are automatically displayed via `react-hot-toast`:

```typescript
✅ "Account created successfully"
❌ "Failed to create account: Invalid account number"
```

### Custom Error Handling

```typescript
const { error } = useAccounts(companyId);

if (error) {
  console.error('Error loading accounts:', error);
  // Display custom error UI
}
```

### API Client Error Handling

The API client automatically handles:

- **401 Unauthorized**: Redirects to `/login`
- **Network Errors**: Retries up to 3 times
- **Timeout**: After 30 seconds

## TypeScript Types

All API responses and requests are fully typed. Import from `types/finance.ts`:

```typescript
import { 
  Account, 
  JournalEntry, 
  Vendor, 
  CreateAccountRequest,
  UpdateAccountRequest,
  // ... more types
} from '@/types/finance';
```

### Type Safety

```typescript
// TypeScript will catch errors at compile time
const account: Account = {
  id: '1',
  companyId: '123',
  accountNumber: '1000',
  accountName: 'Cash',
  type: 'Asset', // Must be valid AccountType enum
  balance: 10000,
  isActive: true,
  isSystemAccount: false,
  isCOGS: false,
  isNonDeductible: false,
  createdAt: '2026-02-05',
  updatedAt: '2026-02-05',
};
```

## Best Practices

### 1. Get Company ID from Auth Context

```typescript
import { useSession } from 'next-auth/react';

const { data: session } = useSession();
const companyId = session?.user?.companyId;
```

### 2. Loading States

Always show loading indicators:

```typescript
if (isLoading) {
  return <LoadingSpinner />;
}
```

### 3. Error Boundaries

Wrap pages in error boundaries:

```typescript
<ErrorBoundary fallback={<ErrorPage />}>
  <FinancePage />
</ErrorBoundary>
```

### 4. Optimistic Updates

For better UX, use optimistic updates:

```typescript
const updateAccount = useMutation({
  mutationFn: accountsApi.update,
  onMutate: async (newData) => {
    // Cancel outgoing refetches
    await queryClient.cancelQueries(['accounts', companyId]);
    
    // Optimistically update the cache
    const previousAccounts = queryClient.getQueryData(['accounts', companyId]);
    queryClient.setQueryData(['accounts', companyId], (old) => {
      // Update logic
    });
    
    return { previousAccounts };
  },
  onError: (err, newData, context) => {
    // Rollback on error
    queryClient.setQueryData(['accounts', companyId], context.previousAccounts);
  },
});
```

## Testing

### Backend API Endpoints

Before using in production, verify all endpoints are available:

1. **Accounts**: `GET /api/v1/finance/accounts?companyId={id}`
2. **Journal Entries**: `GET /api/v1/finance/journal-entries?companyId={id}`
3. **Vendors**: `GET /api/v1/vendors?companyId={id}`
4. **Reports**: `POST /api/v1/reports/financial/profit-and-loss`

### Manual Testing

1. Start backend: `dotnet run` from `src/JERP.Api`
2. Start frontend: `npm run dev` from `landing-page`
3. Navigate to finance pages and verify data loads

### Integration Tests

```typescript
// Example test with React Testing Library
import { renderHook, waitFor } from '@testing-library/react';
import { useAccounts } from '@/hooks/useAccounts';

test('fetches accounts successfully', async () => {
  const { result } = renderHook(() => useAccounts('company-123'));
  
  await waitFor(() => expect(result.current.isLoading).toBe(false));
  
  expect(result.current.accounts).toHaveLength(10);
  expect(result.current.error).toBeNull();
});
```

## Troubleshooting

### Issue: "Cannot read property 'data' of undefined"

**Solution**: Ensure the backend API is running and accessible.

### Issue: "401 Unauthorized"

**Solution**: Check that:
1. Auth token is in localStorage
2. Token is valid and not expired
3. Backend authentication is configured

### Issue: CORS errors

**Solution**: Configure CORS in backend API:

```csharp
// In Program.cs or Startup.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
```

### Issue: "Network Error"

**Solution**: Check:
1. Backend API is running on correct port
2. Firewall/antivirus not blocking requests
3. HTTPS certificates are valid for localhost

## Migration from Mock Data

To migrate existing finance pages from mock data:

1. Remove mock data imports:
   ```typescript
   // DELETE THIS
   import { mockAccounts } from '@/lib/finance/mock-data';
   ```

2. Add hook imports:
   ```typescript
   // ADD THIS
   import { useAccounts } from '@/hooks/useAccounts';
   ```

3. Replace data usage:
   ```typescript
   // BEFORE
   const accounts = mockAccounts;
   
   // AFTER
   const { accounts, isLoading } = useAccounts(companyId);
   ```

4. Add loading and error states

5. Test thoroughly

## Support

For issues or questions:
- Check this README
- Review API documentation in backend `Controllers/`
- Contact: licensing@jerp.io

---

**Last Updated**: 2026-02-05  
**Version**: 1.0.0  
**Author**: JERP Development Team
