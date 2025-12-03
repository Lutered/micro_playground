using AuthAPI.Data.Entities;
using AuthAPI.Models;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.Login
{
    public class LoginCommandHandler(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger
     ) : IRequestHandler<LoginCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDTO = request.DTO;

            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == loginDTO.Username);

            if (user == null) 
                return HandlerResult<AuthResponseDTO>.Failure(HandlerErrorType.NotFound, "Invalid username");

            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result)
                return HandlerResult<AuthResponseDTO>.Failure(HandlerErrorType.NotFound, "Invalid username");

            logger.LogInformation($"User {user.UserName} was login successfuly");

            return HandlerResult<AuthResponseDTO>.Success(new AuthResponseDTO()
            {
                Username = user.UserName,
                Token = await tokenService.GenerateAccessToken(user),
                RefreshToken = await tokenService.GenerateRefreshToken(user)
            });
        }
    }
}
