using MassTransit;
using MediatR;
using Shared.Models.Contracts.User.Requests.GetUser;
using Shared.Models.DTOs.User;
using UsersAPI.Features.Queries.GetUser;

namespace UsersAPI.Consumers
{
    public class GetUserById(IMediator _mediator)
        : IConsumer<GetUserRequest>
    {
        public async Task Consume(ConsumeContext<GetUserRequest> context)
        {
            var contract = context.Message;

            var user = await _mediator.Send(new GetUserQuery(contract.UserId));

            await context.RespondAsync(user.Value);
        }
    }
}
