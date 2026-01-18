namespace Shared.Models.Contracts.User.Requests.CreateUser
{
    public record class CreateUserRequest
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public int Age { get; init; }
    }
}
