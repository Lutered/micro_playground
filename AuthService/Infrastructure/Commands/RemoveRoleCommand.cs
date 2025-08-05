using MediatR;
using Shared;

namespace AuthAPI.Infrastructure.Commands
{
    public class RemoveRoleCommand : IRequest<HandlerResult<bool>>
    {
        public string Username { get; set; }
        public string RoleName { get; set; }

        public RemoveRoleCommand(string username, string rolename)
        {
            Username = username;
            RoleName = rolename;
        }
    }
}
