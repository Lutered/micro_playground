using AuthAPI.Data.Entities;
using AuthAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repositories
{
    public class AuthRepository(AuthContext _context) : IAuthRepository
    {
        public async Task<RefreshToken> GetRefereshToken(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
        }
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
