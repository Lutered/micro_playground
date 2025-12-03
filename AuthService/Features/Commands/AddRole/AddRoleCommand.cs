using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.AddRole
{
    public class AddRoleCommand : IRequest<HandlerResult<bool>>
    {
        public string Username { get; set; }
        public string RoleName { get; set; }

        public AddRoleCommand(string username, string rolename)
        {
            Username = username;
            RoleName = rolename;
        }
    }
}
