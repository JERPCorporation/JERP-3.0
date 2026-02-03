using JERP.Application.DTOs.TaxWithholding;

namespace JERP.Application.Services.TaxWithholding;

public interface ITaxWithholdingService
{
    Task<List<TaxWithholdingDto>> GetByEmployeeAsync(Guid employeeId);
    Task<TaxWithholdingDto> CreateAsync(TaxWithholdingDto dto);
    Task<TaxWithholdingDto?> UpdateAsync(Guid id, TaxWithholdingDto dto);
}
