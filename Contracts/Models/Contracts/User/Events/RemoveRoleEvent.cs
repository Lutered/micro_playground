namespace Shared.Models.Contracts.User.Events
{
    public record class RemoveRoleEvent
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}

