using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models.Requests.Auth
{
    public record class RegisterRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; init; }

        [Required]
        [JsonPropertyName("email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; init; }

        [Required]
        [JsonPropertyName("age")]
        [Range(18, 99)]
        public int Age { get; init; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; init; }
    }
}
