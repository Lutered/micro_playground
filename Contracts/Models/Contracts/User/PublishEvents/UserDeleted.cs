namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class UserDeleted
    {
        public Guid Id { get; init; }
    }
}
