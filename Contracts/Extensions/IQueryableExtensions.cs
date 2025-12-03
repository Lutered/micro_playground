using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;

namespace Shared.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageNum, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNum, pageSize);
        }
    }
}
