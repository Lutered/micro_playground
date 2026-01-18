using Newtonsoft.Json;
using Shared.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Models.Common
{
    public class PagedList<T>
    {
        public PagedList() { }
        public PagedList(IEnumerable<T> items, int pageNum, int pageSize, int totalCount)
        {
            Items = items;
            this.CurrentPage = pageNum;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        [JsonProperty("items")]
        public IEnumerable<T> Items { get; set; } = new List<T>();

        [JsonProperty("page")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }


    }
}
