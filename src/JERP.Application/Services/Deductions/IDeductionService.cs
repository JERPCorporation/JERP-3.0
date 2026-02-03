using JERP.Application.DTOs.Deductions;

namespace JERP.Application.Services.Deductions;

public interface IDeductionService
{
    Task<List<DeductionDto>> GetByEmployeeAsync(Guid employeeId);
    Task<DeductionDto?> GetByIdAsync(Guid id);
    Task<DeductionDto> CreateAsync(DeductionDto dto);
    Task<DeductionDto?> UpdateAsync(Guid id, DeductionDto dto);
    Task<bool> DeleteAsync(Guid id);
}
