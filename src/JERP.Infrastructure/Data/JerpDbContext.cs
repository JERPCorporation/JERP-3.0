using Microsoft.EntityFrameworkCore;
using JERP.Core.Entities;
using JERP.Core.Entities.Finance;
using JERP.Core.Entities.Inventory;
using JERP.Core.Entities.SalesOrders;

namespace JERP.Infrastructure.Data
{
    public class JerpDbContext : DbContext
    {
        // SINGLE CONSTRUCTOR ONLY - SQL Server
        public JerpDbContext(DbContextOptions<JerpDbContext> options) 
            : base(options)
        {
        }

        // DbSets - Core Entities
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        
        // DbSets - Payroll
        public DbSet<Timesheet> Timesheets => Set<Timesheet>();
        public DbSet<PayPeriod> PayPeriods => Set<PayPeriod>();
        public DbSet<PayrollRecord> PayrollRecords => Set<PayrollRecord>();
        public DbSet<PayrollRecordDetail> PayrollRecordDetails => Set<PayrollRecordDetail>();
        public DbSet<TaxWithholding> TaxWithholdings => Set<TaxWithholding>();
        public DbSet<Deduction> Deductions => Set<Deduction>();
        
        // DbSets - Compliance
        public DbSet<ComplianceViolation> ComplianceViolations => Set<ComplianceViolation>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        
        // DbSets - Finance
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
        public DbSet<FASBTopic> FASBTopics => Set<FASBTopic>();
        public DbSet<FASBSubtopic> FASBSubtopics => Set<FASBSubtopic>();
        
        // DbSets - Inventory
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
        
        // DbSets - Sales Orders
        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
        public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
        public DbSet<SOShipment> SOShipments => Set<SOShipment>();
        public DbSet<SOShipmentLine> SOShipmentLines => Set<SOShipmentLine>();
        public DbSet<SalesReturn> SalesReturns => Set<SalesReturn>();
        public DbSet<SalesReturnLine> SalesReturnLines => Set<SalesReturnLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // SQL Server default schema
            modelBuilder.HasDefaultSchema("dbo");
            
            // Apply all entity configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(JerpDbContext).Assembly);
            
            // Seed FASB data
            Seeders.FASBDataSeeder.SeedFASBData(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Auto-update timestamps for BaseEntity
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        baseEntity.CreatedAt = DateTime.UtcNow;
                    }
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
