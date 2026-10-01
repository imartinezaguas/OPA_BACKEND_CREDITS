using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Opa.Credits.Application.Interfaces;
using Opa.Credits.Domain.Entities;
using Opa.Credits.Domain.Enums;
using Opa.Credits.Infrastructure.Persistence;
using Opa.Credits.Infrastructure.Extensions;

namespace Opa.Credits.Infrastructure.Repositories;

public class CreditRepository : ICreditRepository
{
    private readonly ApplicationDbContext _context;

    public CreditRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Credit> GetByIdAsync(int id)
    {
        return await _context.Credits
            .Include(c => c.Associate)
            .Include(c => c.Histories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Credit> Items, int Total)> GetPaginatedAsync(int page, int pageSize, CreditStatus? status = null, string identification = null)
    {
        var query = _context.Credits
            .Include(c => c.Associate)
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(identification))
        {
            query = query.Where(c => c.Associate.Identification == identification);
        }

        return await query
            .OrderByDescending(c => c.RequestDate)
            .PaginarAsync(page, pageSize);
    }

    public async Task<bool> CreditNumberExistsAsync(string creditNumber)
    {
        return await _context.Credits.AnyAsync(c => c.CreditNumber == creditNumber);
    }

    public async Task AddAsync(Credit credit)
    {
        await _context.Credits.AddAsync(credit);
    }

    public Task UpdateAsync(Credit credit)
    {
        _context.Credits.Update(credit);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<CreditStatus, int>> GetCountByStatusAsync()
    {
        var agrupado = await _context.Credits
            .GroupBy(c => c.Status)
            .Select(g => new { Status = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        return agrupado.ToDictionary(k => k.Status, v => v.Cantidad);
    }
}
