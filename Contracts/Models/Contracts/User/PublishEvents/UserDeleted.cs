namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class UserDeleted
    {
        public string Username { get; init; }
    }
}
