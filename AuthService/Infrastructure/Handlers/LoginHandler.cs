using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace AuthAPI.Infrastructure.Handlers
{
    public class LoginHandler(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        ILogger<LoginHandler> logger
     ) : IRequestHandler<LoginCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDTO = request.DTO;

            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == loginDTO.Username);

            if (user == null) 
                return HandlerResult<AuthResponseDTO>.Failure(
                    new HandlerError("Invalid username", HandlerErrorType.Unauthorized));

            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result)
                return HandlerResult<AuthResponseDTO>.Failure(
                    new HandlerError("Invalid username", HandlerErrorType.Unauthorized));

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
