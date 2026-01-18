using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Features.Commands.AddRole
{
    public record class AddRoleCommand : IRequest<HandlerResult>
    {
        public AddRoleRequest Input { get; init; }

        public AddRoleCommand(AddRoleRequest input)
        {
            Input = Input;
        }
    }
}
