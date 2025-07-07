using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTOs
{
    public class PageDTO
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}
