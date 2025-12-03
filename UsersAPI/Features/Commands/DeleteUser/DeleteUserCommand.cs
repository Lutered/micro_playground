using MediatR;
using Shared.Models.Common;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<HandlerResult<bool>>
    {
        public string Username { get; set; }

        public DeleteUserCommand(string username)
        {
            Username = username;
        }
    }
}
