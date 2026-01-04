using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<HandlerResult<UserDTO>>
    {
        public UserDTO Input { get; set; }

        public CreateUserCommand(UserDTO input) 
        {
            Input = input;
        }
    }
}
