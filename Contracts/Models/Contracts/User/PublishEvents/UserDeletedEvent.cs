namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class UserDeletedEvent
    {
        public Guid Id { get; init; }
    }
}
