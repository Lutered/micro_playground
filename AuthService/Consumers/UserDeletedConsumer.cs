using AuthAPI.Features.Commands.DeleteUser;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;

namespace AuthAPI.Consumers
{
    public class UserDeletedConsumer(IMediator _mediator) : IConsumer<UserDeletedEvent>
    {
        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var contract = context.Message;

            await _mediator.Send(new DeleteUserCommand(contract.Id));
        }
    }
}
