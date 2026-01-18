using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests.Auth
{
    public record class LoginRequest
    {
        [Required]
        public string Username { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
