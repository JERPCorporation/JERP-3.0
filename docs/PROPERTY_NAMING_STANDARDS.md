# JERP 3.0 Property Naming Standards

**Version:** 1.0  
**Last Updated:** February 6, 2026  
**Author:** Julio Cesar Mendez Tobar  
**Copyright:** © 2026 Julio Cesar Mendez Tobar. All Rights Reserved.

---

## Overview

This document establishes the official property naming conventions for the JERP 3.0 codebase. These standards ensure consistency across DTOs, Entities, and database mappings, preventing serialization issues and improving code maintainability.

---

## Core Conventions

### 1. PascalCase for All Properties

**Rule:** All properties must use PascalCase - first letter of each word capitalized, no underscores, no camelCase.

✅ **CORRECT:**
```csharp
public decimal TaxAmount { get; set; }
public string FirstName { get; set; }
public DateTime DueDate { get; set; }
```

❌ **INCORRECT:**
```csharp
public decimal taxAmount { get; set; }      // camelCase
public decimal tax_amount { get; set; }     // snake_case
public decimal TAXAMOUNT { get; set; }      // ALL CAPS
```

---

### 2. Common Compound Words - Single Capital

**Rule:** Treat commonly recognized compound words as single words.

✅ **CORRECT (Common Compounds):**
```csharp
public decimal Subtotal { get; set; }      // NOT SubTotal
public string Username { get; set; }       // NOT UserName
public string Email { get; set; }          // NOT EMail
public string Filename { get; set; }       // NOT FileName
public DateTime Timestamp { get; set; }    // NOT TimeStamp
```

✅ **CORRECT (Separate Concepts):**
```csharp
public decimal TaxAmount { get; set; }     // Tax + Amount (two concepts)
public string FirstName { get; set; }      // First + Name (two concepts)
public DateTime DueDate { get; set; }      // Due + Date (two concepts)
public Guid CompanyId { get; set; }        // Company + Id (two concepts)
```

**Reference:** Microsoft's .NET Framework Design Guidelines

---

### 3. Boolean Properties - Use "Is/Has/Can" Prefix

**Rule:** Boolean properties should use descriptive prefixes that indicate their nature.

✅ **CORRECT:**
```csharp
public bool IsActive { get; set; }
public bool IsDeleted { get; set; }
public bool IsPosted { get; set; }
public bool IsVoid { get; set; }
public bool HasAttachments { get; set; }
public bool CanEdit { get; set; }
public bool RequiresMetrcTracking { get; set; }
```

❌ **INCORRECT:**
```csharp
public bool Active { get; set; }           // Missing prefix
public bool Deleted { get; set; }          // Missing prefix
public bool Posted { get; set; }           // Missing prefix
```

**Exception:** When the boolean IS the primary state (e.g., `Status` enum)

---

### 4. ID Properties - Always "Id" Not "ID"

**Rule:** Use "Id" with lowercase 'd' for all identifier properties.

✅ **CORRECT:**
```csharp
public Guid Id { get; set; }
public Guid CompanyId { get; set; }
public Guid EmployeeId { get; set; }
public Guid VendorId { get; set; }
public Guid CustomerId { get; set; }
```

❌ **INCORRECT:**
```csharp
public Guid ID { get; set; }
public Guid CompanyID { get; set; }
public Guid Company_Id { get; set; }
```

---

### 5. Date Properties - [Descriptor]Date Pattern

**Rule:** Date properties should follow the pattern: descriptive noun + "Date"

✅ **CORRECT:**
```csharp
public DateTime BillDate { get; set; }     // Date of bill
public DateTime DueDate { get; set; }      // Date when due
public DateTime PostedDate { get; set; }   // Date when posted
public DateTime CreatedDate { get; set; }  // Date created
public DateTime HireDate { get; set; }     // Date hired
public DateTime OrderDate { get; set; }    // Date ordered
public DateTime InvoiceDate { get; set; }  // Date invoiced
public DateTime ReturnDate { get; set; }   // Date returned
```

❌ **INCORRECT:**
```csharp
public DateTime DateBill { get; set; }     // Backwards
public DateTime Date { get; set; }         // Too generic
public DateTime BillDt { get; set; }       // Abbreviation
```

**Note:** Properties like `CreatedAt`, `UpdatedAt`, `ApprovedAt` are also acceptable for timestamp fields.

---

### 6. Amount/Money Properties - [Type]Amount Pattern

**Rule:** Monetary properties should follow: type + "Amount" or just the noun if context is clear.

✅ **CORRECT:**
```csharp
public decimal Subtotal { get; set; }      // Exception: common term
public decimal TaxAmount { get; set; }
public decimal TotalAmount { get; set; }
public decimal DiscountAmount { get; set; }
public decimal PaymentAmount { get; set; }
public decimal ShippingAmount { get; set; }
public decimal AmountPaid { get; set; }
public decimal AmountDue { get; set; }
```

✅ **ALSO ACCEPTABLE:**
```csharp
public decimal Total { get; set; }         // When context is clear
public decimal Tax { get; set; }           // In tax-specific context
```

---

## Entity-Specific Standards

### SalesOrder Entity

```csharp
public class SalesOrder : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CustomerId { get; set; }
    public string SONumber { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequestedShipDate { get; set; }
    public DateTime? PromisedShipDate { get; set; }
    
    // Amounts
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Boolean flags
    public bool IsFullyShipped { get; set; }
    public bool IsFullyInvoiced { get; set; }
    public bool RequiresMetrcTracking { get; set; }
}
```

### SalesReturn Entity

```csharp
public class SalesReturn : BaseEntity
{
    public Guid Id { get; set; }
    public string RMANumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? SalesOrderId { get; set; }
    public DateTime ReturnDate { get; set; }
    
    // Amounts
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
```

### PurchaseOrder Entity

```csharp
public class PurchaseOrder : BaseEntity
{
    public Guid Id { get; set; }
    public string PONumber { get; set; }
    public Guid VendorId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    
    // Amounts
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Boolean flags
    public bool IsFullyReceived { get; set; }
}
```

### VendorBill/Invoice Entities

```csharp
public class VendorBill : BaseEntity
{
    public Guid Id { get; set; }
    public string BillNumber { get; set; }
    public Guid VendorId { get; set; }
    public DateTime BillDate { get; set; }
    public DateTime DueDate { get; set; }
    
    // Amounts
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    
    // Boolean flags
    public bool IsPaid { get; set; }
}
```

---

## DTO Consistency

### Rule: DTOs Must Match Entity Property Names

**Critical:** DTOs must use identical property names as their corresponding entities to ensure seamless mapping.

```csharp
// Entity
public class SalesOrder : BaseEntity
{
    public decimal Subtotal { get; set; }
}

// DTO - MUST match
public class SalesOrderDto
{
    public decimal Subtotal { get; set; }  // ✅ Same name!
}
```

---

## Database Mappings

### Entity Framework Configuration

When configuring entities with Fluent API, ensure column names follow the same conventions:

```csharp
public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.Property(e => e.Subtotal)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("Subtotal");  // ✅ Matches property name
    }
}
```

---

## Migration from Old Standards

### Changes Made (February 2026)

The following changes were made to standardize the codebase:

#### SubTotal → Subtotal

**Files Changed:**
- `src/JERP.Core/Entities/SalesOrders/SalesOrder.cs`
- `src/JERP.Core/Entities/SalesOrders/SalesReturn.cs`
- `src/JERP.Application/DTOs/SalesOrders/SalesOrderDto.cs`
- `src/JERP.Application/DTOs/SalesOrders/SalesReturnDto.cs`
- `src/JERP.Application/DTOs/SalesOrders/CustomerInvoiceDto.cs`
- `src/JERP.Application/DTOs/PurchaseOrders/PurchaseOrderDto.cs`
- `src/JERP.Application/DTOs/PurchaseOrders/VendorBillDto.cs`
- `src/JERP.Application/DTOs/Finance/AccountsPayableDtos.cs`
- `src/JERP.Application/DTOs/Finance/AccountsReceivableDtos.cs`
- `src/JERP.Application/Services/SalesOrders/SalesOrderService.cs`
- `src/JERP.Application/Services/SalesOrders/SalesReturnService.cs`
- `src/JERP.Application/Services/PurchaseOrders/PurchaseOrderService.cs`
- `src/JERP.Application/Services/PurchaseOrders/VendorBillService.cs`
- `src/JERP.Desktop/ViewModels/Finance/InvoicesViewModel.cs`
- `src/JERP.Desktop/ViewModels/Finance/BillsViewModel.cs`

**Total:** 15 files, 23 occurrences updated

---

## Compliance Check

### Pre-Development Checklist

Before adding new properties, verify:

- [ ] Property uses PascalCase
- [ ] Common compound words use single capital (Subtotal, Username, Email)
- [ ] Boolean properties have Is/Has/Can prefix
- [ ] ID properties use "Id" not "ID"
- [ ] Date properties follow [Descriptor]Date pattern
- [ ] Amount properties follow [Type]Amount pattern
- [ ] DTO matches corresponding Entity property names
- [ ] Database column names match property names

---

## Common Patterns Reference

### Financial Properties
```csharp
public decimal Subtotal { get; set; }
public decimal TaxAmount { get; set; }
public decimal TotalAmount { get; set; }
public decimal DiscountAmount { get; set; }
public decimal ShippingAmount { get; set; }
public decimal AmountPaid { get; set; }
public decimal AmountDue { get; set; }
```

### Date Properties
```csharp
public DateTime OrderDate { get; set; }
public DateTime InvoiceDate { get; set; }
public DateTime BillDate { get; set; }
public DateTime DueDate { get; set; }
public DateTime PostedDate { get; set; }
public DateTime CreatedDate { get; set; }
public DateTime HireDate { get; set; }
public DateTime ReturnDate { get; set; }
public DateTime? ExpectedDeliveryDate { get; set; }
```

### Identifier Properties
```csharp
public Guid Id { get; set; }
public Guid CompanyId { get; set; }
public Guid CustomerId { get; set; }
public Guid VendorId { get; set; }
public Guid EmployeeId { get; set; }
public Guid ProductId { get; set; }
public Guid WarehouseId { get; set; }
public Guid AccountId { get; set; }
```

### Boolean Flags
```csharp
public bool IsActive { get; set; }
public bool IsDeleted { get; set; }
public bool IsPosted { get; set; }
public bool IsVoid { get; set; }
public bool IsPaid { get; set; }
public bool IsFullyShipped { get; set; }
public bool IsFullyInvoiced { get; set; }
public bool IsFullyReceived { get; set; }
public bool HasAttachments { get; set; }
public bool CanEdit { get; set; }
public bool RequiresMetrcTracking { get; set; }
```

### Number Properties
```csharp
public string SONumber { get; set; }        // Sales Order Number
public string PONumber { get; set; }        // Purchase Order Number
public string RMANumber { get; set; }       // Return Merchandise Authorization
public string InvoiceNumber { get; set; }
public string BillNumber { get; set; }
```

---

## Benefits

Following these standards provides:

✅ **Consistency** - Uniform naming across entire codebase  
✅ **Maintainability** - Easy to understand and modify  
✅ **Reliability** - Prevents serialization issues  
✅ **Professionalism** - Industry-standard conventions  
✅ **Developer Productivity** - No guessing property names  
✅ **Code Quality** - Easier code reviews and onboarding  

---

## Enforcement

### Code Review Guidelines

During code reviews, verify:

1. All new properties follow these conventions
2. No abbreviations (except common acronyms like RMA, PO, SO)
3. No underscores or camelCase
4. Boolean properties have appropriate prefixes
5. DTOs match their corresponding entities

### Automated Checks

Consider adding:
- Linting rules for property naming
- Unit tests that verify DTO ↔ Entity mapping
- Static analysis tools to catch violations

---

## References

- [Microsoft Framework Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines)
- [C# Naming Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [Entity Framework Core Conventions](https://learn.microsoft.com/en-us/ef/core/modeling/)

---

## Questions or Exceptions?

If you encounter a naming scenario not covered by these standards, or believe an exception is warranted:

1. Document the use case
2. Propose the naming convention
3. Submit for team review
4. Update this document if approved

---

**Document History:**
- v1.0 - February 6, 2026 - Initial release with SubTotal → Subtotal standardization

---

© 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
