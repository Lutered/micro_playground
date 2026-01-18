using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 25;

        [FromQuery(Name = "sort")]
        public string Sort { get; set; } = string.Empty;
    }
}
