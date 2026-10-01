using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Opa.Credits.Application.Interfaces;
using Opa.Credits.Domain.Entities;
using Opa.Credits.Infrastructure.Persistence;

namespace Opa.Credits.Infrastructure.Repositories;

public class AssociateRepository : IAssociateRepository
{
    private readonly ApplicationDbContext _context;

    public AssociateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Associate> GetByIdentificationAsync(string identification)
    {
        return await _context.Associates
            .FirstOrDefaultAsync(a => a.Identification == identification);
    }

    public async Task AddAsync(Associate associate)
    {
        await _context.Associates.AddAsync(associate);
    }
}
