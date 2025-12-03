namespace Shared.Models.Contracts.Requests.User
{
    public record class UserDeleted
    {
        public string Username { get; init; }
    }
}
