using AuthAPI.Features.Commands.AddRole;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;

namespace AuthAPI.Consumers
{
    public class AddRoleConsumer(IMediator mediator) : IConsumer<AddRole>
    {
        public async Task Consume(ConsumeContext<AddRole> context)
        {
            var contract = context.Message;

            await mediator.Send(new AddRoleCommand(contract.Username, contract.RoleName));
        }
    }
}
