using AuthAPI.Data.Entities;
using Shared.Models.Responses.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;
using AuthAPI.Services.Interfaces;

namespace AuthAPI.Features.Commands.Login
{
    public class LoginCommandHandler(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger
     ) : IRequestHandler<LoginCommand, HandlerResult<AuthResponse>>
    {
        public async Task<HandlerResult<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken = default)
        {
            var input = request.Input;

            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == input.Username, cancellationToken);

            if (user == null)
                return GetAuthError(input.Username);

            var result = await userManager.CheckPasswordAsync(user, input.Password);

            if (!result)
                return GetAuthError(input.Username);

            logger.LogInformation($"User {user.UserName} was login successfuly");

            return HandlerResult<AuthResponse>.Success(new AuthResponse()
            {
                Username = user.UserName,
                Token = await tokenService.GenerateAccessToken(user),
                RefreshToken = await tokenService.GenerateRefreshToken(user)
            });
        }

        private HandlerResult<AuthResponse> GetAuthError(string username)
        {
            logger.LogDebug($"Auth error for user {username}");
            return HandlerResult<AuthResponse>
                    .Failure(HandlerErrorType.NotFound, "Invalid username or password");
        }
    }
}
