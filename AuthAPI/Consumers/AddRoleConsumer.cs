using AuthAPI.Features.Commands.AddRole;
using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.Events;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Consumers
{
    public class AddRoleConsumer(IMediator mediator, IMapper _mapper) : IConsumer<AddRoleEvent>
    {
        public async Task Consume(ConsumeContext<AddRoleEvent> context)
        {
            var contract = context.Message;

            await mediator.Send(new AddRoleCommand(_mapper.Map<AddRoleRequest>(contract)));
        }
    }
}
