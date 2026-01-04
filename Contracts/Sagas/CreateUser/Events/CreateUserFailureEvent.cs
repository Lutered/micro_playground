namespace Shared.Sagas.CreateUser.Events
{
    public class CreateUserFailureEvent : BaseEvent
    {
        public Guid Id { get; init; }
    }
}
