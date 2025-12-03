namespace Shared.Models.Contracts.Requests.User
{
    public record class RemoveRole
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}

