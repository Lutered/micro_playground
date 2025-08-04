using AuthAPI.Controllers;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Extensions;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator;
using AuthAPI.Mediator.Commands;
using AutoMapper;
using Contracts.Requests.User;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.MediatR.Commands.Handlers
{
    public class RegisterCommandHandler(
        UserManager<AppUser> userManager,
        IMapper mapper,
        ITokenService tokenService,
        IPublishEndpoint publishEndpoint,
        ILogger<RegisterCommandHandler> logger
     ) : IRequestHandler<RegisterCommand, Result<AuthResponseDTO>>
    {
        public async Task<Result<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDTO = request.DTO;

            if (await userManager.UserExists(registerDTO.Username))
                return Result<AuthResponseDTO>.Failure(
                        new AppError(
                            $"User with username {registerDTO.Username} already exists", 
                            ErrorType.Conflict));

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return Result<AuthResponseDTO>.Failure(
                    new AppError(
                        string.Join("; ", result.Errors.Select(e => e.Description)), 
                        null));
            
            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            logger.LogInformation($"User {user.UserName} was created successfuly");

            if (!result.Succeeded)
                return Result<AuthResponseDTO>.Failure(
                    new AppError(
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)),
                        null));

            await publishEndpoint.Publish(new UserCreated
            {
                Id = new Guid(),
                Username = user.UserName,
                Email = user.Email,
                Age = user.Age
            });

            return Result<AuthResponseDTO>.Success(new AuthResponseDTO()
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user)
            });
        }
    }
}
