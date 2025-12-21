using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests
{
    public class PagedRequest
    {
        [MinLength(1)]
        public int Page { get; set; } = 1;

        [MinLength(1)]
        [MaxLength(100)]
        public int PageSize { get; set; } = 25;

        public string Sort { get; set; }
    }
}
