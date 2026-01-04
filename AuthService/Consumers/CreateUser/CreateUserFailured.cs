using MassTransit;

namespace AuthAPI.Consumers.CreateUser
{
    public class CreateUserFailured : IConsumer<CreateUserFailured>
    {
        public Task Consume(ConsumeContext<CreateUserFailured> context)
        {
            throw new NotImplementedException();
        }
    }
}
