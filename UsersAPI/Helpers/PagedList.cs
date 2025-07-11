﻿using Microsoft.EntityFrameworkCore;

namespace UsersAPI.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNum, int pageSize)
        {
            this.CurrentPage = pageNum;
            this.PageSize = pageSize;
            this.TotalCount = count;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int pageNum, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNum, pageSize);
        }
    }
}
