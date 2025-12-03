using AuthAPI.Features.Commands.RemoveRole;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.Requests.User;

namespace AuthAPI.Consumers
{
    public class RemoveRoleConsumer(IMediator mediator) : IConsumer<RemoveRole>
    {
        public async Task Consume(ConsumeContext<RemoveRole> context)
        {
            var contract = context.Message;

            await mediator.Send(new RemoveRoleCommand(contract.Username, contract.RoleName));
        }
    }
}
