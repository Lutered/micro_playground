using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Features.Commands.Login
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
