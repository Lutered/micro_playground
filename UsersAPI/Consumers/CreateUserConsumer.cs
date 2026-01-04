using AutoMapper;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.Requests.CreateUser;
using Shared.Models.DTOs.User;
using UsersAPI.Features.Commands.CreateUser;

namespace UsersAPI.Consumers
{
    public class CreateUserConsumer(
        IMediator _mediator,
        IMapper _mapper)
      : IConsumer<CreateUserRequest>
    {
        public async Task Consume(ConsumeContext<CreateUserRequest> context)
        {
            var contract = context.Message;

            try
            {
                var result = await _mediator.Send(new CreateUserCommand(
                    _mapper.Map<UserDTO>(contract)
                ));

                if (!result.IsSuccess) 
                    throw new Exception(result.Error.Message);

                await context.RespondAsync(new CreateUserResponse() 
                {
                    Success = true,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex) 
            {
                await context.RespondAsync(new CreateUserResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
