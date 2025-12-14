using AuthAPI.Data;
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
     ) : IRequestHandler<RefreshCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _authRepo.GetRefereshToken(request.Token);

            if (storedToken == null || storedToken.IsUsed
                || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
                return HandlerResult<AuthResponseDTO>.Failure(HandlerErrorType.BadRequest, "Token is not valid");

            storedToken.IsUsed = true;
            //_context.RefreshTokens.Update(storedToken);

            var user = storedToken.User;
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = new RefreshToken
            {
                UserId = storedToken.UserId,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            await _authRepo.AddRefreshToken(newRefreshToken);
            await _authRepo.SaveChangesAsync();

            _logger.LogInformation($"Refresh token for user {user.UserName} was updated successfuly");

            return HandlerResult<AuthResponseDTO>.Success(new AuthResponseDTO
            {
                Username = user.UserName,
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }
    }
}
