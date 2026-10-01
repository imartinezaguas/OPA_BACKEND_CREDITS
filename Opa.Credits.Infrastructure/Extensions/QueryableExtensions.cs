using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Opa.Credits.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<(IEnumerable<T> Items, int Total)> PaginarAsync<T>(this IQueryable<T> query, int pagina, int cantidad)
    {
        int total = await query.CountAsync();
        var items = await query.Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
        return (items, total);
    }
}
