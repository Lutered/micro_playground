using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests.Auth
{
    public record class RegisterRequest
    {
        [Required]
        public string Username { get; init; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; init; }

        [Required]
        [Range(18, 99)]
        public int Age { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
