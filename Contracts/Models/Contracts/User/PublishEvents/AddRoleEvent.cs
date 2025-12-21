namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class AddRoleEvent
    {
        public string Username { get; init; }
        public string RoleName { get; init; }
    }
}
