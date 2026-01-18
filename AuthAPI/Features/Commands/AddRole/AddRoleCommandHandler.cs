using AuthAPI.Data.Entities;
using AuthAPI.Features.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.AddRole
{
    public class AddRoleCommandHandler(
        UserManager<AppUser> userManager,
        ILogger<LoginCommandHandler> logger
        )
        : IRequestHandler<AddRoleCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName.ToLower() == input.Username.ToLower());

            if (user is null) 
                return HandlerResult.Failure(HandlerErrorType.NotFound, "User was not found");

            try
            {
                await userManager.AddToRoleAsync(user, input.RoleName);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return HandlerResult.Success();
        }
    }
}
