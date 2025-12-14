using AuthAPI.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(UserManager<AppUser> userManager)
        : IRequestHandler<DeleteUserCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return HandlerResult.Failure(HandlerErrorType.NotFound, $"User with id {id} was not found");

            await userManager.DeleteAsync(user);

            return HandlerResult.Success();
        }
    }
}
