using MediatR;
using Shared.Models.Common;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<HandlerResult>
    {
        public Guid Id { get; set; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
