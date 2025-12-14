using AuthAPI.Data.Entities;
using Shared.Models.Responses.Auth;
using AuthAPI.Features.Commands.Login;
using MediatR;
using Shared.Models.Common;
using AuthAPI.Data.Repositories.Interfaces;
using AuthAPI.Services.Interfaces;

namespace AuthAPI.Features.Commands.Refresh
{
    public class RefreshCommandHandler(
        IAuthRepository _authRepo,
        ITokenService _tokenService,
        ILogger<LoginCommandHandler> _logger
     ) : IRequestHandler<RefreshCommand, HandlerResult<AuthResponse>>
    {
        public async Task<HandlerResult<AuthResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken = default)
        {
            var storedToken = await _authRepo.GetRefereshToken(request.Token, cancellationToken);

            if (storedToken == null || storedToken.IsUsed
                || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
                return HandlerResult<AuthResponse>.Failure(HandlerErrorType.BadRequest, "Token is not valid");

            storedToken.IsUsed = true;

            var user = storedToken.User;

            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken(user, cancellationToken);

            _logger.LogInformation($"Refresh token for user {user.UserName} was updated successfuly");

            return HandlerResult<AuthResponse>.Success(new AuthResponse
            {
                Username = user.UserName,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
