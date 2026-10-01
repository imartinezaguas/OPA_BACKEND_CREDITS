using System.Collections.Generic;
using System.Threading.Tasks;
using Opa.Credits.Domain.Entities;
using Opa.Credits.Domain.Enums;

namespace Opa.Credits.Application.Interfaces;

public interface ICreditRepository
{
    Task<Credit> GetByIdAsync(int id);
    Task<(IEnumerable<Credit> Items, int Total)> GetPaginatedAsync(int page, int pageSize, CreditStatus? status = null, string identification = null);
    Task<bool> CreditNumberExistsAsync(string creditNumber);
    Task AddAsync(Credit credit);
    Task UpdateAsync(Credit credit);
    Task SaveChangesAsync();
    Task<Dictionary<CreditStatus, int>> GetCountByStatusAsync();
}
