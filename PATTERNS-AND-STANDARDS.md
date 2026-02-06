# JERP 3.0 - Established Patterns & Standards

**Version:** 3.0  
**Last Updated:** 2026-02-06  
**Purpose:** Document architectural patterns and coding standards established during enterprise quality audit

---

## 1. DTO Status Property Pattern

### Standard Pattern (Dual Property)

For DTOs that require both programmatic status checking and display formatting:

```csharp
/// <summary>
/// Example DTO with dual status properties
/// </summary>
public class ExampleDto
{
    // ... other properties ...
    
    /// <summary>
    /// Status enum for programmatic logic and type safety
    /// </summary>
    public ExampleStatus StatusEnum { get; set; }
    
    /// <summary>
    /// Status string for display purposes and serialization
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    // ... other properties ...
}
```

### Service Mapping Pattern

When mapping from Entity to DTO, populate both properties:

```csharp
private ExampleDto MapToDto(Example entity)
{
    return new ExampleDto
    {
        // ... other property mappings ...
        
        StatusEnum = entity.Status,        // Enum value
        Status = entity.Status.ToString(), // String representation
        
        // ... other property mappings ...
    };
}
```

### Applied To

✅ **TimesheetDto** - Original implementation  
✅ **PayPeriodDto** - Original implementation  
✅ **BillDto** - Applied during audit  
✅ **InvoiceDto** - Applied during audit  
✅ **BillListDto** - Applied during audit  
✅ **InvoiceListDto** - Applied during audit

### Recommended For

⚠️ All DTOs with status fields (SalesOrderDto, PurchaseOrderDto, etc.)

---

## 2. DTO Property Naming Standards

### Subtotal vs SubTotal

**Standard:** Use `Subtotal` as primary property name

**For Backward Compatibility:**
```csharp
public decimal Subtotal { get; set; }
public decimal SubTotal { get; set; } // Alias for backward compatibility
```

**Future Recommendation:** Use only `Subtotal` in new DTOs

### Navigation Property Naming

**For Entity References:**
```csharp
// Entity ID
public Guid DepartmentId { get; set; }

// Display name from navigation
public string? DepartmentName { get; set; }  // ✅ Correct pattern

// NOT: Department (conflicts with entity navigation)
// NOT: DepartmentName on Entity (should be Name)
```

**Entity Structure:**
```csharp
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;  // ✅ Use "Name" not "DepartmentName"
}
```

**DTO Mapping:**
```csharp
// In Service
DepartmentName = employee.Department?.Name  // ✅ Correct
```

---

## 3. Entity → DTO Mapping Standards

### Complete Mapping Template

```csharp
private ExampleDto MapToDto(Example entity)
{
    return new ExampleDto
    {
        // 1. Primary properties
        Id = entity.Id,
        CompanyId = entity.CompanyId,
        
        // 2. Business properties
        Name = entity.Name,
        Description = entity.Description,
        Amount = entity.Amount,
        
        // 3. Status (dual property pattern)
        StatusEnum = entity.Status,
        Status = entity.Status.ToString(),
        
        // 4. Navigation properties (IDs + display names)
        ParentId = entity.ParentId,
        ParentName = entity.Parent?.Name,
        
        // 5. Collections (if needed)
        LineItems = entity.LineItems.Select(MapLineItemToDto).ToList(),
        
        // 6. Audit fields
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}
```

### Verified Mappings

| Entity | DTO | Service | Status |
|--------|-----|---------|--------|
| Bill | BillDto | APService | ✅ Complete |
| Invoice | InvoiceDto | ARService | ✅ Complete |
| Employee | EmployeeDto | EmployeeService | ✅ Complete |
| Timesheet | TimesheetDto | TimesheetService | ✅ Complete |
| PayrollRecord | PayrollRecordDto | PayrollService | ✅ Complete |

---

## 4. Enum Usage Standards

### Enum Definitions

**Location:** `src/JERP.Core/Enums/`

**Standard Structure:**
```csharp
namespace JERP.Core.Enums;

/// <summary>
/// Status values for Example entities
/// </summary>
public enum ExampleStatus
{
    /// <summary>
    /// Initial draft state
    /// </summary>
    Draft = 0,
    
    /// <summary>
    /// Pending approval
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// Approved and active
    /// </summary>
    Approved = 2,
    
    /// <summary>
    /// Voided/cancelled
    /// </summary>
    Void = 99
}
```

### Available Enums

**Finance:**
- BillStatus
- InvoiceStatus
- JournalEntryStatus
- AccountType
- EntrySource
- PaymentMethod

**HR/Payroll:**
- EmployeeStatus
- EmploymentType
- EmployeeClassification
- PayrollStatus
- TimesheetStatus
- PayFrequency
- DeductionType

**Inventory:**
- SalesOrderStatus
- SalesReturnStatus
- PurchaseOrderStatus
- ShipmentStatus
- AdjustmentStatus
- TransferStatus
- CountType
- InventoryTransactionType

**Compliance:**
- ViolationStatus
- ViolationSeverity
- ViolationType

---

## 5. Service Method Patterns

### Standard Service Method Structure

```csharp
/// <summary>
/// Gets entity by ID with full details
/// </summary>
public async Task<Result<ExampleDto>> GetByIdAsync(Guid id)
{
    try
    {
        // 1. Retrieve entity with necessary includes
        var entity = await _context.Examples
            .Include(e => e.Parent)
            .Include(e => e.LineItems)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        // 2. Null check
        if (entity == null)
        {
            return Result<ExampleDto>.Failure("Entity not found");
        }
        
        // 3. Map to DTO
        var dto = MapToDto(entity);
        
        // 4. Return success result
        return Result<ExampleDto>.Success(dto);
    }
    catch (Exception ex)
    {
        // 5. Log and return failure
        _logger.LogError(ex, "Error retrieving entity {Id}", id);
        return Result<ExampleDto>.Failure($"Error retrieving entity: {ex.Message}");
    }
}
```

### List/Search Method Pattern

```csharp
/// <summary>
/// Gets filtered list of entities
/// </summary>
public async Task<Result<List<ExampleListDto>>> GetListAsync(
    Guid companyId,
    ExampleStatus? status = null,
    string? search = null)
{
    try
    {
        // 1. Start with base query (multi-tenant filtered)
        var query = _context.Examples
            .Where(e => e.CompanyId == companyId);
        
        // 2. Apply filters
        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e => e.Name.Contains(search));
        }
        
        // 3. Order and project to DTO
        var results = await query
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new ExampleListDto
            {
                Id = e.Id,
                Name = e.Name,
                StatusEnum = e.Status,
                Status = e.Status.ToString(),
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();
        
        // 4. Return success
        return Result<List<ExampleListDto>>.Success(results);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving list for company {CompanyId}", companyId);
        return Result<List<ExampleListDto>>.Failure($"Error retrieving list: {ex.Message}");
    }
}
```

---

## 6. API Controller Patterns

### Standard Controller Structure

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Manages Example entities
/// </summary>
[Authorize]  // ✅ Always include for protected resources
[ApiController]
[Route("api/v1/[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly IExampleService _service;
    private readonly ILogger<ExamplesController> _logger;
    
    public ExamplesController(
        IExampleService service,
        ILogger<ExamplesController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    /// <summary>
    /// Gets entity by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ExampleDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ExampleDto>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to get entity {Id}: {Error}", id, result.Error);
            return NotFound(result.Error);
        }
        
        return Ok(result.Data);
    }
    
    /// <summary>
    /// Gets filtered list
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ExampleListDto>), 200)]
    public async Task<ActionResult<List<ExampleListDto>>> GetList(
        [FromQuery] Guid companyId,
        [FromQuery] ExampleStatus? status = null,
        [FromQuery] string? search = null)
    {
        var result = await _service.GetListAsync(companyId, status, search);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Data);
    }
}
```

### Multi-Tenant Pattern

```csharp
// Extract company ID from authenticated user
var companyId = User.GetCompanyId();

// Always filter by company
var result = await _service.GetListAsync(companyId, ...);
```

---

## 7. Desktop ViewModel Patterns

### Standard ViewModel Structure

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JERP.Desktop.ViewModels;

public partial class ExamplesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;
    
    [ObservableProperty]
    private ObservableCollection<ExampleDto> _items = new();
    
    [ObservableProperty]
    private ExampleDto? _selectedItem;
    
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private ExampleStatus? _statusFilter;
    
    public ExamplesViewModel(
        IApiClient apiClient,
        IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = LoadDataAsync();
    }
    
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        
        IsBusy = true;
        ErrorMessage = null;
        
        try
        {
            var companyId = _authService.CurrentUser?.CompanyId;
            if (companyId == null)
            {
                SetError("Company not found");
                return;
            }
            
            var queryString = $"api/v1/examples?companyId={companyId}";
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                queryString += $"&search={Uri.EscapeDataString(SearchText)}";
            }
            
            if (StatusFilter.HasValue)
            {
                queryString += $"&status={StatusFilter.Value}";
            }
            
            var response = await _apiClient.GetAsync<List<ExampleDto>>(queryString);
            
            Items.Clear();
            if (response != null)
            {
                foreach (var item in response)
                {
                    Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

### ViewModel → DTO Mapping (Dynamic API Response)

When API returns dynamic JSON, map carefully:

```csharp
// ✅ Correct: Map dynamic to DTO with proper type conversions
var dto = new ExampleDto
{
    Id = item.id,
    Name = item.name,
    StatusEnum = (ExampleStatus)item.status,  // Convert to enum
    Status = item.status?.ToString() ?? string.Empty,  // Convert to string
    Amount = item.amount
};
```

---

## 8. Async/Await Standards

### ✅ Correct Patterns

```csharp
// Service method
public async Task<Result<T>> GetAsync()
{
    var result = await _repository.GetAsync();
    return Result<T>.Success(result);
}

// Controller method
public async Task<ActionResult<T>> GetAsync()
{
    var result = await _service.GetAsync();
    return Ok(result.Data);
}

// ViewModel command
[RelayCommand]
private async Task LoadAsync()
{
    var data = await _apiClient.GetAsync<T>(...);
    Items = data;
}
```

### ❌ Avoid Anti-Patterns

```csharp
// ❌ DON'T use .Result (deadlock risk)
var result = _service.GetAsync().Result;

// ❌ DON'T use .Wait() (deadlock risk)
_service.GetAsync().Wait();

// ❌ DON'T mix async and sync
public async Task<T> GetAsync()
{
    var result = _repository.Get(); // Missing await
    return result;
}
```

---

## 9. Error Handling Standards

### Result Pattern

```csharp
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    
    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };
    
    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}
```

### Service Error Handling

```csharp
try
{
    // Operation
    return Result<T>.Success(data);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Operation failed");
    return Result<T>.Failure($"Error: {ex.Message}");
}
```

### Controller Error Handling

```csharp
var result = await _service.DoSomethingAsync();

if (!result.IsSuccess)
{
    _logger.LogWarning("Operation failed: {Error}", result.Error);
    return BadRequest(result.Error);
}

return Ok(result.Data);
```

---

## 10. Security Standards

### Authentication

```csharp
// ✅ All controllers (except Auth)
[Authorize]
[ApiController]
public class ExampleController : ControllerBase

// ✅ Auth controller - selective authorization
[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]  // Public login
    public async Task<IActionResult> Login() { }
    
    [Authorize]  // Protected logout
    public async Task<IActionResult> Logout() { }
}
```

### Multi-Tenant Isolation

```csharp
// ✅ Always filter by CompanyId
var query = _context.Examples
    .Where(e => e.CompanyId == companyId);

// ✅ Extract from authenticated user
var companyId = User.GetCompanyId();
```

### Input Validation

```csharp
// DTO level
public class CreateExampleDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }
}

// Service level
if (dto == null)
{
    return Result.Failure("Invalid input");
}
```

---

## 11. Logging Standards

### Log Levels

```csharp
// Information - normal operations
_logger.LogInformation("Created entity {Id}", entity.Id);

// Warning - handled errors
_logger.LogWarning("Entity {Id} not found", id);

// Error - exceptions
_logger.LogError(ex, "Failed to process {Operation}", "Create");

// Debug - detailed diagnostics (non-production)
_logger.LogDebug("Processing {Count} items", items.Count);
```

### Structured Logging

```csharp
// ✅ Use structured parameters
_logger.LogInformation("User {UserId} performed {Action} on {EntityType} {EntityId}",
    userId, action, entityType, entityId);

// ❌ Avoid string interpolation in logs
_logger.LogInformation($"User {userId} performed {action}");
```

---

## 12. Database Standards

### Entity Configuration

```csharp
public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        // Table name
        builder.ToTable("Examples");
        
        // Primary key
        builder.HasKey(e => e.Id);
        
        // Required fields
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        // Decimals with precision
        builder.Property(e => e.Amount)
            .HasPrecision(18, 2);
        
        // Indexes
        builder.HasIndex(e => new { e.CompanyId, e.Status });
        
        // Relationships
        builder.HasOne(e => e.Company)
            .WithMany(c => c.Examples)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### Repository Pattern

```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
```

---

## Summary

These patterns and standards have been established and verified during the enterprise quality audit of JERP 3.0. They represent best practices for:

✅ Type safety with enums  
✅ Consistent DTO structures  
✅ Clean service layer  
✅ Secure API endpoints  
✅ Maintainable desktop ViewModels  
✅ Proper error handling  
✅ Multi-tenant security  

**Apply these patterns consistently in all new development.**

---

**Document Version:** 1.0  
**Last Reviewed:** 2026-02-06  
**Next Review:** After next major feature release
