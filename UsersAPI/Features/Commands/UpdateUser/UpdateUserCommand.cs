using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.User;

namespace UsersAPI.Features.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<HandlerResult>
    {
        public UpdateUserRequest Input { get; set; }

        public UpdateUserCommand(UpdateUserRequest input)
        {
            Input = input;
        }
    }
}
