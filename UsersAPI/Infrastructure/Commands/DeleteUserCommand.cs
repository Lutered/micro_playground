using MediatR;
using Shared;

namespace UsersAPI.Infrastructure.Commands
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
