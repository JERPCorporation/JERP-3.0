/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Text.Json;
using JERP.Core.Entities;
using JERP.Core.Entities.Finance;
using JERP.Core.Entities.Inventory;
using JERP.Infrastructure.Data.Configurations;
using JERP.Infrastructure.Data.Configurations.Finance;
using JERP.Infrastructure.Data.Configurations.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JERP.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the JERP system
/// </summary>
public class JerpDbContext : DbContext
{
    public JerpDbContext(DbContextOptions<JerpDbContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Timesheet> Timesheets => Set<Timesheet>();
    public DbSet<PayPeriod> PayPeriods => Set<PayPeriod>();
    public DbSet<PayrollRecord> PayrollRecords => Set<PayrollRecord>();
    public DbSet<PayrollRecordDetail> PayrollRecordDetails => Set<PayrollRecordDetail>();
    public DbSet<TaxWithholding> TaxWithholdings => Set<TaxWithholding>();
    public DbSet<Deduction> Deductions => Set<Deduction>();
    public DbSet<ComplianceViolation> ComplianceViolations => Set<ComplianceViolation>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    
    // Finance module DbSets
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<GeneralLedgerEntry> GeneralLedgerEntries => Set<GeneralLedgerEntry>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<VendorBill> VendorBills => Set<VendorBill>();
    public DbSet<BillLineItem> BillLineItems => Set<BillLineItem>();
    public DbSet<BillPayment> BillPayments => Set<BillPayment>();
    public DbSet<CustomerInvoice> CustomerInvoices => Set<CustomerInvoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();
    
    // Inventory module DbSets
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductBatch> ProductBatches => Set<ProductBatch>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<InventoryLevel> InventoryLevels => Set<InventoryLevel>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<StockReceipt> StockReceipts => Set<StockReceipt>();
    public DbSet<StockReceiptLine> StockReceiptLines => Set<StockReceiptLine>();
    public DbSet<InventoryAdjustment> InventoryAdjustments => Set<InventoryAdjustment>();
    public DbSet<InventoryAdjustmentLine> InventoryAdjustmentLines => Set<InventoryAdjustmentLine>();
    public DbSet<PhysicalCount> PhysicalCounts => Set<PhysicalCount>();
    public DbSet<PhysicalCountLine> PhysicalCountLines => Set<PhysicalCountLine>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<StockTransferLine> StockTransferLines => Set<StockTransferLine>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    /// <summary>
    /// Configures entity mappings and relationships
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Set default schema
        modelBuilder.HasDefaultSchema("dbo");
        
        // Apply all entity configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new TimesheetConfiguration());
        modelBuilder.ApplyConfiguration(new PayPeriodConfiguration());
        modelBuilder.ApplyConfiguration(new PayrollRecordConfiguration());
        modelBuilder.ApplyConfiguration(new PayrollRecordDetailConfiguration());
        modelBuilder.ApplyConfiguration(new TaxWithholdingConfiguration());
        modelBuilder.ApplyConfiguration(new DeductionConfiguration());
        modelBuilder.ApplyConfiguration(new ComplianceViolationConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        
        // Apply finance module configurations
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new JournalEntryConfiguration());
        modelBuilder.ApplyConfiguration(new GeneralLedgerEntryConfiguration());
        modelBuilder.ApplyConfiguration(new VendorConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new VendorBillConfiguration());
        
        // Apply inventory module configurations
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductBatchConfiguration());
        modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryLevelConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderLineConfiguration());
        modelBuilder.ApplyConfiguration(new StockReceiptConfiguration());
        modelBuilder.ApplyConfiguration(new StockReceiptLineConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryAdjustmentConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryAdjustmentLineConfiguration());
        modelBuilder.ApplyConfiguration(new PhysicalCountConfiguration());
        modelBuilder.ApplyConfiguration(new PhysicalCountLineConfiguration());
        modelBuilder.ApplyConfiguration(new StockTransferConfiguration());
        modelBuilder.ApplyConfiguration(new StockTransferLineConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryTransactionConfiguration());
        
        // SQL Server specific optimizations
        ConfigureSqlServerSpecifics(modelBuilder);
    }

    /// <summary>
    /// Applies SQL Server-specific configurations
    /// </summary>
    private void ConfigureSqlServerSpecifics(ModelBuilder modelBuilder)
    {
        // Set default string lengths to prevent nvarchar(max)
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(string) && p.GetMaxLength() == null))
        {
            property.SetMaxLength(256);
        }
        
        // Use datetime2 instead of datetime for better precision
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetColumnType("datetime2");
        }
    }

    /// <summary>
    /// Overrides SaveChangesAsync to automatically handle timestamps, soft deletes, and audit logging
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || 
                       e.State == EntityState.Modified || 
                       e.State == EntityState.Deleted)
            .ToList();

        var auditEntries = new List<AuditLog>();
        var currentUserId = GetCurrentUserId();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    
                    if (currentUserId.HasValue)
                    {
                        auditEntries.Add(CreateAuditLog(entry, "Create", currentUserId.Value));
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    
                    if (currentUserId.HasValue)
                    {
                        auditEntries.Add(CreateAuditLog(entry, "Update", currentUserId.Value));
                    }
                    break;

                case EntityState.Deleted:
                    // Implement soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    
                    if (currentUserId.HasValue)
                    {
                        auditEntries.Add(CreateAuditLog(entry, "Delete", currentUserId.Value));
                    }
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        // Save audit logs after successful save
        if (auditEntries.Any())
        {
            await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Creates an audit log entry for an entity change
    /// </summary>
    private AuditLog CreateAuditLog(EntityEntry<BaseEntity> entry, string action, Guid userId)
    {
        var entityType = entry.Entity.GetType().Name;
        var entityId = entry.Entity.Id;

        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        if (action == "Update")
        {
            foreach (var property in entry.Properties)
            {
                if (property.IsModified && 
                    property.Metadata.Name != nameof(BaseEntity.UpdatedAt))
                {
                    oldValues[property.Metadata.Name] = property.OriginalValue;
                    newValues[property.Metadata.Name] = property.CurrentValue;
                }
            }
        }
        else if (action == "Create")
        {
            foreach (var property in entry.Properties)
            {
                newValues[property.Metadata.Name] = property.CurrentValue;
            }
        }
        else if (action == "Delete")
        {
            foreach (var property in entry.Properties)
            {
                oldValues[property.Metadata.Name] = property.OriginalValue;
            }
        }

        var auditLog = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            OldValues = oldValues.Any() ? JsonSerializer.Serialize(oldValues) : null,
            NewValues = newValues.Any() ? JsonSerializer.Serialize(newValues) : null,
            Timestamp = DateTime.UtcNow,
            CurrentHash = GenerateHash(entityType, entityId, action)
        };

        return auditLog;
    }

    /// <summary>
    /// Generates a hash for audit log integrity verification
    /// </summary>
    private string GenerateHash(string entityType, Guid entityId, string action)
    {
        var input = $"{entityType}:{entityId}:{action}:{DateTime.UtcNow.Ticks}";
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// Gets the current user ID from the context (to be implemented with authentication)
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        // TODO: Implement retrieval of current user ID from authentication context
        // For now, returns null. This should be injected via IHttpContextAccessor or similar
        return null;
    }
}
