namespace Shared.Models.Contracts.User.Events
{
    public record class UserDeletedEvent
    {
        public Guid Id { get; init; }
    }
}
