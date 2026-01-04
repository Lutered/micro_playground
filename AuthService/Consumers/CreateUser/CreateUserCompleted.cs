using MassTransit;

namespace AuthAPI.Consumers.CreateUser
{
    public class CreateUserCompleted : IConsumer<CreateUserCompleted>
    {
        public Task Consume(ConsumeContext<CreateUserCompleted> context)
        {
            throw new NotImplementedException();
        }
    }
}
