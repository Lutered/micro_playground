using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
