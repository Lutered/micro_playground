using AuthAPI.Features.Commands.DeleteUser;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;

namespace AuthAPI.Consumers
{
    public class UserDeletedConsumer(IMediator _mediator) : IConsumer<UserDeleted>
    {
        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            var contract = context.Message;

            await _mediator.Send(new DeleteUserCommand(contract.Id));
        }
    }
}
