# Finance Module API Integration - Example Implementation

This document shows **before and after** examples of converting a finance page from mock data to real API integration.

## Example: Vendors Page

### Before (Using Mock Data)

```typescript
"use client";

import { useState } from 'react';
import { mockVendors } from '@/lib/finance/mock-data';
import { Vendor } from '@/lib/finance/types';

export default function VendorsPage() {
  const [searchTerm, setSearchTerm] = useState('');
  
  // ❌ Using static mock data
  const filteredVendors = mockVendors.filter(vendor =>
    vendor.name.toLowerCase().includes(searchTerm.toLowerCase())
  );
  
  const totalBalance = mockVendors.reduce((sum, v) => sum + v.balance, 0);
  
  return (
    <div>
      <h1>Vendors</h1>
      <input
        type="text"
        placeholder="Search vendors..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <div>
        {filteredVendors.map((vendor) => (
          <div key={vendor.id}>
            <h3>{vendor.name}</h3>
            <p>Balance: ${vendor.balance}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
```

### After (Using API Integration)

```typescript
"use client";

import { useState, useMemo } from 'react';
import { useVendors } from '@/hooks/useVendors';
import { useSession } from 'next-auth/react';

export default function VendorsPage() {
  const [searchTerm, setSearchTerm] = useState('');
  const [showCreateModal, setShowCreateModal] = useState(false);
  
  // ✅ Get company ID from auth session
  const { data: session } = useSession();
  const companyId = session?.user?.companyId || '';
  
  // ✅ Use the API hook
  const { 
    vendors, 
    isLoading, 
    error, 
    createVendor, 
    updateVendor, 
    deleteVendor,
    isCreating,
    isDeleting
  } = useVendors(companyId);
  
  // ✅ Client-side filtering
  const filteredVendors = useMemo(() => {
    if (!vendors) return [];
    return vendors.filter(vendor =>
      vendor.companyName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      (vendor.email && vendor.email.toLowerCase().includes(searchTerm.toLowerCase()))
    );
  }, [vendors, searchTerm]);
  
  // ✅ Calculate totals
  const totalBalance = useMemo(() => {
    return vendors?.reduce((sum, v) => sum + v.balance, 0) || 0;
  }, [vendors]);
  
  // ✅ Handle create vendor
  const handleCreateVendor = (data: any) => {
    createVendor({
      companyName: data.companyName,
      contactPerson: data.contactPerson,
      email: data.email,
      phone: data.phone,
      address: data.address,
    });
    setShowCreateModal(false);
  };
  
  // ✅ Handle delete vendor
  const handleDeleteVendor = (id: string) => {
    if (confirm('Are you sure you want to delete this vendor?')) {
      deleteVendor(id);
    }
  };
  
  // ✅ Loading state
  if (isLoading) {
    return (
      <div style={{ 
        minHeight: "100vh", 
        display: "flex", 
        alignItems: "center", 
        justifyContent: "center",
        background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)"
      }}>
        <div style={{ textAlign: "center" }}>
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-500 mx-auto mb-4"></div>
          <p style={{ color: "#94a3b8" }}>Loading vendors...</p>
        </div>
      </div>
    );
  }
  
  // ✅ Error state
  if (error) {
    return (
      <div style={{ 
        minHeight: "100vh", 
        display: "flex", 
        alignItems: "center", 
        justifyContent: "center",
        background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)"
      }}>
        <div style={{ 
          padding: "24px", 
          background: "rgba(239, 68, 68, 0.1)", 
          border: "1px solid rgba(239, 68, 68, 0.3)",
          borderRadius: "12px",
          maxWidth: "500px"
        }}>
          <h2 style={{ color: "#f87171", marginBottom: "8px" }}>Error Loading Vendors</h2>
          <p style={{ color: "#fca5a5" }}>{error.message}</p>
          <button 
            onClick={() => window.location.reload()}
            style={{
              marginTop: "16px",
              padding: "8px 16px",
              background: "#ef4444",
              color: "white",
              border: "none",
              borderRadius: "6px",
              cursor: "pointer"
            }}
          >
            Retry
          </button>
        </div>
      </div>
    );
  }
  
  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)",
      padding: "40px 20px"
    }}>
      <div style={{ maxWidth: "1400px", margin: "0 auto" }}>
        <h1 style={{ color: "#f1f5f9", fontSize: "36px", marginBottom: "32px" }}>
          🏢 Vendors
        </h1>
        
        {/* Stats */}
        <div style={{ marginBottom: "32px", display: "flex", gap: "20px" }}>
          <div style={{ 
            padding: "20px", 
            background: "rgba(30, 41, 59, 0.9)", 
            borderRadius: "12px",
            flex: 1
          }}>
            <p style={{ color: "#94a3b8", fontSize: "14px" }}>Total Vendors</p>
            <p style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700" }}>
              {vendors?.length || 0}
            </p>
          </div>
          <div style={{ 
            padding: "20px", 
            background: "rgba(30, 41, 59, 0.9)", 
            borderRadius: "12px",
            flex: 1
          }}>
            <p style={{ color: "#94a3b8", fontSize: "14px" }}>Total Balance</p>
            <p style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700" }}>
              ${totalBalance.toLocaleString()}
            </p>
          </div>
        </div>
        
        {/* Search and Actions */}
        <div style={{ marginBottom: "20px", display: "flex", gap: "12px" }}>
          <input
            type="text"
            placeholder="Search vendors..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            style={{
              flex: 1,
              padding: "12px 16px",
              background: "rgba(30, 41, 59, 0.9)",
              border: "1px solid rgba(71, 85, 105, 0.3)",
              borderRadius: "8px",
              color: "#f1f5f9",
              fontSize: "14px"
            }}
          />
          <button
            onClick={() => setShowCreateModal(true)}
            disabled={isCreating}
            style={{
              padding: "12px 24px",
              background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
              color: "white",
              border: "none",
              borderRadius: "8px",
              cursor: isCreating ? "not-allowed" : "pointer",
              opacity: isCreating ? 0.6 : 1
            }}
          >
            {isCreating ? "Creating..." : "+ Add Vendor"}
          </button>
        </div>
        
        {/* Vendors List */}
        <div style={{
          background: "rgba(30, 41, 59, 0.9)",
          borderRadius: "12px",
          padding: "24px"
        }}>
          {filteredVendors.length === 0 ? (
            <p style={{ color: "#94a3b8", textAlign: "center", padding: "40px" }}>
              {searchTerm ? "No vendors found matching your search." : "No vendors yet. Add your first vendor to get started."}
            </p>
          ) : (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px" }}>
                    VENDOR
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px" }}>
                    CONTACT
                  </th>
                  <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px" }}>
                    BALANCE
                  </th>
                  <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px" }}>
                    STATUS
                  </th>
                  <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px" }}>
                    ACTIONS
                  </th>
                </tr>
              </thead>
              <tbody>
                {filteredVendors.map((vendor) => (
                  <tr 
                    key={vendor.id}
                    style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.2)" }}
                  >
                    <td style={{ padding: "16px", color: "#f1f5f9" }}>
                      <div style={{ fontWeight: "600" }}>{vendor.companyName}</div>
                      <div style={{ fontSize: "12px", color: "#94a3b8" }}>
                        {vendor.vendorNumber}
                      </div>
                    </td>
                    <td style={{ padding: "16px", color: "#94a3b8", fontSize: "13px" }}>
                      <div>{vendor.contactPerson || "—"}</div>
                      <div>{vendor.email || "—"}</div>
                    </td>
                    <td style={{ padding: "16px", textAlign: "right", color: "#f1f5f9", fontWeight: "600" }}>
                      ${vendor.balance.toLocaleString()}
                    </td>
                    <td style={{ padding: "16px", textAlign: "center" }}>
                      <span style={{
                        padding: "4px 8px",
                        borderRadius: "4px",
                        fontSize: "11px",
                        fontWeight: "500",
                        background: vendor.isActive ? "rgba(34, 197, 94, 0.2)" : "rgba(148, 163, 184, 0.2)",
                        color: vendor.isActive ? "#4ade80" : "#94a3b8"
                      }}>
                        {vendor.isActive ? "Active" : "Inactive"}
                      </span>
                    </td>
                    <td style={{ padding: "16px", textAlign: "right" }}>
                      <button
                        onClick={() => handleDeleteVendor(vendor.id)}
                        disabled={isDeleting}
                        style={{
                          padding: "6px 12px",
                          background: "rgba(239, 68, 68, 0.1)",
                          border: "1px solid rgba(239, 68, 68, 0.3)",
                          borderRadius: "6px",
                          color: "#ef4444",
                          fontSize: "12px",
                          cursor: isDeleting ? "not-allowed" : "pointer",
                          opacity: isDeleting ? 0.6 : 1
                        }}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
      
      {/* Create Modal would go here */}
    </div>
  );
}
```

## Key Changes Made

### 1. Import Changes
```typescript
// ❌ Before
import { mockVendors } from '@/lib/finance/mock-data';

// ✅ After
import { useVendors } from '@/hooks/useVendors';
import { useSession } from 'next-auth/react';
```

### 2. Data Source
```typescript
// ❌ Before
const vendors = mockVendors;

// ✅ After
const { vendors, isLoading, error } = useVendors(companyId);
```

### 3. Added Loading State
```typescript
if (isLoading) {
  return <LoadingSpinner />;
}
```

### 4. Added Error Handling
```typescript
if (error) {
  return <ErrorMessage error={error} />;
}
```

### 5. Real CRUD Operations
```typescript
// Create
createVendor({ companyName: "...", ... });

// Update
updateVendor({ id: "123", data: { isActive: false } });

// Delete
deleteVendor("123");
```

## Migration Checklist

When converting a page from mock data to API integration:

- [ ] Import the appropriate hook(s)
- [ ] Get `companyId` from auth session
- [ ] Replace mock data with hook data
- [ ] Add loading state UI
- [ ] Add error state UI
- [ ] Update CRUD operations to use mutations
- [ ] Add disabled states during mutations
- [ ] Test with backend API running
- [ ] Handle edge cases (empty lists, etc.)
- [ ] Remove mock data imports

## Testing the Integration

1. **Start Backend API**
   ```bash
   cd src/JERP.Api
   dotnet run
   ```

2. **Start Frontend**
   ```bash
   cd landing-page
   npm run dev
   ```

3. **Test Scenarios**
   - Page loads with real data
   - Loading spinner shows initially
   - Error message displays if backend is offline
   - Create operation works and updates list
   - Update operation works and reflects changes
   - Delete operation works and removes item
   - Toast notifications appear for success/error

## Common Issues

### Issue: "companyId is undefined"
**Solution**: User must be logged in. Add auth check:
```typescript
if (!companyId) {
  return <div>Please log in to view vendors.</div>;
}
```

### Issue: "vendors is undefined"
**Solution**: Use fallback value:
```typescript
const vendors = vendorsData || [];
```

### Issue: Infinite loading
**Solution**: Check that backend API is running and accessible.

---

This example demonstrates the complete migration pattern that can be applied to all finance pages.
