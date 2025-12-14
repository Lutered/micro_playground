namespace Shared.Models.Requests.Auth
{
    public record class AddRoleRequest
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}
