using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;

namespace UsersAPI.Features.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<HandlerResult<UserDTO>>
    {
        public Guid Id { get; set; }
        public UpdateUserRequest Input { get; set; }

        public UpdateUserCommand(Guid id, UpdateUserRequest input)
        {
            Id = id;
            Input = input;
        }
    }
}
