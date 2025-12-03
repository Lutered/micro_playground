using Microsoft.EntityFrameworkCore;

namespace Shared.Models.Common
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int pageNum, int pageSize, int totalCount)
        {
            this.CurrentPage = pageNum;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
