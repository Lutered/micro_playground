using AuthAPI.Features.Commands.RemoveRole;
using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Consumers
{
    public class RemoveRoleConsumer(IMediator mediator, IMapper _mapper) : IConsumer<RemoveRole>
    {
        public async Task Consume(ConsumeContext<RemoveRole> context)
        {
            var contract = context.Message;

            await mediator.Send(new RemoveRoleCommand(_mapper.Map<RemoveRoleRequest>(contract)));
        }
    }
}
