using AuthAPI.Features.Commands.DeleteUser;
using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.Events;
using Shared.Models.Contracts.User.Requests.CreateUser;
using Shared.Models.Contracts.User.Requests.DeleteUser;
using Shared.Models.DTOs.User;

namespace AuthAPI.Consumers
{
    public class UserDeletedConsumer(IMediator _mediator) : IConsumer<UserDeletedEvent>
    {
        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var contract = context.Message;

            try
            {
                var result = await _mediator.Send(new DeleteUserCommand(contract.Id));

                if (!result.IsSuccess)
                    throw new Exception(result.Error.Message);

                await context.RespondAsync(new DeleteUserResponse()
                {
                    Success = true,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new DeleteUserResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
