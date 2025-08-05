using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.MediatR.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace AuthAPI.Infrastructure.Handlers
{
    public class RefreshHandler(
        AppDbContext _context,
        ITokenService _tokenService,
        ILogger<LoginHandler> _logger
     ) : IRequestHandler<RefreshCommand, HandlerResult<AuthResponseDTO>>
    {
        public async Task<HandlerResult<AuthResponseDTO>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _context.RefreshTokens
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Token == request.Token);

            if (storedToken == null || storedToken.IsUsed
                || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
                return HandlerResult<AuthResponseDTO>.Failure(new AppError("Token is not valid"));

            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);

            var user = storedToken.User;
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = new RefreshToken
            {
                UserId = storedToken.UserId,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Refresh token for user {user.UserName} was updated successfuly");

            return HandlerResult< AuthResponseDTO >.Success(new AuthResponseDTO
            {
                Username = user.UserName,
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }
    }
}
