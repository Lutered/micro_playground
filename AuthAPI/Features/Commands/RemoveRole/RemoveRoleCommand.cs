using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Features.Commands.RemoveRole
{
    public record class RemoveRoleCommand : IRequest<HandlerResult>
    {
        public RemoveRoleRequest Input { get; init; }

        public RemoveRoleCommand(RemoveRoleRequest input)
        {
            Input = input;
        }
    }
}
