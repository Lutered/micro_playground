using AuthAPI.Data.Entities;
using AuthAPI.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace AuthAPI.Infrastructure.Handlers
{
    public class RemoveRoleHandler(
        UserManager<AppUser> userManager,
        ILogger<LoginHandler> logger
        )
        : IRequestHandler<RemoveRoleCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == request.Username.ToLower());

            if (user is null)
                return HandlerResult<bool>.Failure(new AppError("User was not found", ErrorType.NotFound));

            try
            {
                await userManager.RemoveFromRoleAsync(user, request.RoleName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return HandlerResult<bool>.Failure(new AppError(ex.Message, ErrorType.Internal));
            }

            return HandlerResult<bool>.Success(true);
        }
    }
}
