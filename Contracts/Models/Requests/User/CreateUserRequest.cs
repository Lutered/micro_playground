namespace Shared.Models.Requests.User
{
    public class CreateUserRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
