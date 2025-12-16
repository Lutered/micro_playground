using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.PublishEvents;
using Shared.Models.DTOs.User;
using UsersAPI.Features.Commands.CreateUser;

namespace UsersAPI.Consumers
{
    public class UserCreatedConsumer(
        IMediator _mediator,
        IMapper _mapper)
      : IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var contract = context.Message;

            await _mediator.Send(new CreateUserCommand(
                _mapper.Map<UserDTO>(contract)
            ));
        }
    }
}
