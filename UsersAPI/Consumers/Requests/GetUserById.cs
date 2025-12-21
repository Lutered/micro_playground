using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.Requests;
using Shared.Models.DTOs.User;
using UsersAPI.Features.Queries.GetUser;

namespace UsersAPI.Consumers.Requests
{
    public class GetUserById(IMediator _mediator)
        : IConsumer<GetUserRequest>
    {
        public async Task Consume(ConsumeContext<GetUserRequest> context)
        {
            var contract = context.Message;

            var user = await _mediator.Send(new GetUserQuery(contract.UserId));

            await context.RespondAsync<UserDTO>(user.Value);
        }
    }
}
