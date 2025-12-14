namespace Shared.Models.Contracts.User.PublishEvents
{
    public record class UserCreated
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public int Age { get; init; }
    }
}
