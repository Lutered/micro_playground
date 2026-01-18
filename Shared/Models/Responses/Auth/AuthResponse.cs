using System.Text.Json.Serialization;

namespace Shared.Models.Responses.Auth
{
    public record class AuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; init; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; init; }

        [JsonPropertyName("username")]
        public string Username { get; init; }
    }
}
