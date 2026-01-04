using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;
using System.Linq.Expressions;

namespace Shared.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageNum, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, pageNum, pageSize, count);
        }

        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query, 
            string sort, 
            Dictionary<string, Expression<Func<T, object>>> sortMap)
        {
            var sortingValues = sort.Split(',');

            IOrderedQueryable<T> orderedQuery = null;
            foreach(var sortingValueRaw in sortingValues)
            {
                var sortingValue = sortingValueRaw.Trim();
                bool isDesc = sortingValue.EndsWith("_desc");
                bool isAsc = sortingValue.EndsWith("_asc");

                int length = sortingValue.Length;
                if (isDesc) sortingValue = sortingValue.Remove(length - 6, length - 1);
                else if (isAsc) sortingValue = sortingValue.Remove(length - 5, length - 1);

                if (!sortMap.TryGetValue(sortingValue, out var expression)) continue;

                if (orderedQuery is null) orderedQuery = isDesc ? query.OrderByDescending(expression) : query.OrderBy(expression);
                else orderedQuery = isDesc ? orderedQuery.ThenByDescending(expression) : orderedQuery.ThenBy(expression);
            }

            return orderedQuery ?? query;
        }
    }
}
