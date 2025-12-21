namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class RemoveRoleEvent
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}

