using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Features.Commands.Register
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [Required]
        [Range(18, 99)]
        public int Age { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
