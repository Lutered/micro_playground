using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.RemoveRole
{
    public class RemoveRoleCommand : IRequest<HandlerResult<bool>>
    {
        public string Username { get; private set; }
        public string RoleName { get; private set; }

        public RemoveRoleCommand(string username, string rolename)
        {
            Username = username;
            RoleName = rolename;
        }
    }
}
