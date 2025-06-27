using Contracts;
using MassTransit;

namespace UsersAPI.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var contract = context.Message;

            string userName = contract.Username;
        }
    }
}
