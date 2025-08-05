using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Extensions;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator.Commands;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared;
using Shared.Contracts;

namespace AuthAPI.Infrastructure.Handlers
{
    public class RegisterHandler(
        UserManager<AppUser> _userManager,
        IMapper _mapper,
        ITokenService _tokenService,
        IPublishEndpoint _publishEndpoint,
        ILogger<RegisterHandler> _logger
     ) : IRequestHandler<RegisterCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDTO = request.DTO;

            if (await _userManager.UserExistsAsync(registerDTO.Username))
                return HandlerResult<AuthResponseDTO>.Failure(
                        new AppError(
                            $"User with username {registerDTO.Username} already exists", 
                            ErrorType.Conflict));

            var user = _mapper.Map<AppUser>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return HandlerResult<AuthResponseDTO>.Failure(
                    new AppError(
                        string.Join("; ", result.Errors.Select(e => e.Description)), 
                        null));

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return HandlerResult<AuthResponseDTO>.Failure(
                    new AppError(
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)),
                        null));

            _logger.LogInformation($"User {user.UserName} was created successfuly");

            await _publishEndpoint.Publish(new UserCreated
            {
                Username = user.UserName,
                Email = user.Email,
                Age = user.Age
            });

            return HandlerResult<AuthResponseDTO>.Success(new AuthResponseDTO()
            {
                Username = user.UserName,
                Token = await _tokenService.GenerateAccessToken(user),
                RefreshToken = await _tokenService.GenerateRefreshToken(user)
            });
        }
    }
}
