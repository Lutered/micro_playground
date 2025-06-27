using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Range(18, 99)]
        public int Age { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
