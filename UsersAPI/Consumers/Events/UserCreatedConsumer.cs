using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;
using Shared.Models.DTOs.User;
using UsersAPI.Features.Commands.CreateUser;

namespace UsersAPI.Consumers.Events
{
    public class UserCreatedConsumer(
        IMediator _mediator,
        IMapper _mapper)
      : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var contract = context.Message;

            await _mediator.Send(new CreateUserCommand(
                _mapper.Map<UserDTO>(contract)
            ));
        }
    }
}
