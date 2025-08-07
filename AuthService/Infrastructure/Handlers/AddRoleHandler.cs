using AuthAPI.Data.Entities;
using AuthAPI.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace AuthAPI.Infrastructure.Handlers
{
    public class AddRoleHandler(
        UserManager<AppUser> userManager,
        ILogger<LoginHandler> logger
        )
        : IRequestHandler<AddRoleCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == request.Username.ToLower());

            if (user is null) 
                return HandlerResult<bool>.Failure(new HandlerError("User was not found", HandlerErrorType.NotFound));

            try
            {
                await userManager.AddToRoleAsync(user, request.RoleName);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return HandlerResult<bool>.Failure(new HandlerError(ex.Message, HandlerErrorType.Internal)); 
            }

            return HandlerResult<bool>.Success(true);
        }
    }
}
