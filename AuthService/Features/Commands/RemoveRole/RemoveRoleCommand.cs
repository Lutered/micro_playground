using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.RemoveRole
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
