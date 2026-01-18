using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.DeleteUser
{
    public record class DeleteUserCommand : IRequest<HandlerResult>
    {
        public Guid Id { get; init; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
