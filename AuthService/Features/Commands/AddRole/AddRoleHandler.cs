using AuthAPI.Data.Entities;
using AuthAPI.Features.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.AddRole
{
    public class AddRoleHandler(
        UserManager<AppUser> userManager,
        ILogger<LoginCommandHandler> logger
        )
        : IRequestHandler<AddRoleCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == request.Username.ToLower());

            if (user is null) 
                return HandlerResult<bool>.Failure(HandlerErrorType.NotFound, "User was not found");

            try
            {
                await userManager.AddToRoleAsync(user, request.RoleName);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return HandlerResult<bool>.Success(true);
        }
    }
}
