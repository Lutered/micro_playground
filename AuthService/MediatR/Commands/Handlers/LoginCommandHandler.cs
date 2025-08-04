using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Extensions;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator;
using AutoMapper;
using Contracts.Requests.User;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.MediatR.Commands.Handlers
{
    public class LoginCommandHandler(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger
     ) : IRequestHandler<LoginCommand, Result<AuthResponseDTO>>
    {
        public async Task<Result<AuthResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDTO = request.DTO;

            var user = await userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

            if (user == null) 
                return Result<AuthResponseDTO>.Failure(
                    new AppError("Invalid username", ErrorType.Unauthorized));

            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result)
                return Result<AuthResponseDTO>.Failure(
                    new AppError("Invalid username", ErrorType.Unauthorized));

            logger.LogInformation($"User {user.UserName} was login successfuly");

            return Result<AuthResponseDTO>.Success(new AuthResponseDTO()
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user)
            });
        }
    }
}
