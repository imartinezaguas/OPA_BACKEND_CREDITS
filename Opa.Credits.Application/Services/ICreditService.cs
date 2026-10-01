using System.Threading.Tasks;
using Opa.Credits.Application.DTOs;

namespace Opa.Credits.Application.Services;

public interface ICreditService
{
    Task<CreditResponseDto> CreateCreditAsync(CreateCreditDto dto);
    Task<PaginationResponseDto<CreditResponseDto>> GetPaginatedAsync(CreditFilterDto filter);
    Task<CreditDetailDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateCreditDto dto);
    Task ChangeStatusAsync(int id, ChangeStatusDto dto);
    Task DeleteAsync(int id);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
}
