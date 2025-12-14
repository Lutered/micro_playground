namespace Shared.Models.Requests.Auth
{
    public class RemoveRoleRequest
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}
