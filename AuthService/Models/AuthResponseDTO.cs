namespace AuthAPI.Models
{
    public record class AuthResponseDTO
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
        public string Username { get; init; }
    }
}
