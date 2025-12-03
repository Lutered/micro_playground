using AuthAPI.Data.Entities;
using AuthAPI.Models;
using AuthAPI.Extensions;
using AuthAPI.Intrefaces;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Common;
using Shared.Models.Contracts.Requests.User;

namespace AuthAPI.Features.Commands.Register
{
    public class RegisterCommandHandler(
        UserManager<AppUser> _userManager,
        IMapper _mapper,
        ITokenService _tokenService,
        IPublishEndpoint _publishEndpoint,
        ILogger<RegisterCommandHandler> _logger
     ) : IRequestHandler<RegisterCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDTO = request.DTO;

            if (await _userManager.UserExistsAsync(registerDTO.Username))
                return HandlerResult<AuthResponseDTO>.Failure(HandlerErrorType.Conflict, $"User with username {registerDTO.Username} already exists");

            var user = _mapper.Map<AppUser>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return HandlerResult<AuthResponseDTO>.Failure(
                    HandlerErrorType.BadRequest, 
                    string.Join("; ", result.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return HandlerResult<AuthResponseDTO>.Failure(
                    HandlerErrorType.BadRequest, 
                    string.Join("; ", roleResult.Errors.Select(e => e.Description)));

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
