namespace Shared.Contracts
{
    public record class UserCreated
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public int Age { get; init; }
    }
}
