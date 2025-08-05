namespace Shared.Contracts.Requests.User
{
    public record class AddRole
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}
