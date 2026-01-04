namespace Shared.Sagas.CreateUser.Events
{
    public class CreateUserEvent
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public int Age { get; init; }
    }
}
