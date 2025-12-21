using AuthAPI.Data.Entities;
using Shared.Models.Responses.Auth;
using AuthAPI.Extensions;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Common;
using AuthAPI.Services.Interfaces;
using Shared.Models.Contracts.User.PublishEvents;

namespace AuthAPI.Features.Commands.Register
{
    public class RegisterCommandHandler(
        UserManager<AppUser> _userManager,
        IMapper _mapper,
        ITokenService _tokenService,
        IPublishEndpoint _publishEndpoint,
        ILogger<RegisterCommandHandler> _logger
     ) : IRequestHandler<RegisterCommand, HandlerResult<AuthResponse>>
    {
        public async Task<HandlerResult<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
        {
            var input = request.Input;

            if (await _userManager.UserExistsAsync(input.Username))
                return HandlerResult<AuthResponse>.Failure(
                    HandlerErrorType.Conflict, 
                    $"User with username {input.Username} already exists");

            var user = _mapper.Map<AppUser>(input);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return HandlerResult<AuthResponse>.Failure(
                    HandlerErrorType.BadRequest, 
                    string.Join("; ", result.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return HandlerResult<AuthResponse>.Failure(
                    HandlerErrorType.BadRequest, 
                    string.Join("; ", roleResult.Errors.Select(e => e.Description)));

            _logger.LogInformation($"User {user.UserName} was created successfuly");

            await _publishEndpoint.Publish(new UserCreatedEvent
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Age = user.Age
            });

            return HandlerResult<AuthResponse>.Success(new AuthResponse()
            {
                Username = user.UserName,
                Token = await _tokenService.GenerateAccessToken(user),
                RefreshToken = await _tokenService.GenerateRefreshToken(user)
            });
        }
    }
}
