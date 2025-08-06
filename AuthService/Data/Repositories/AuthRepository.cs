using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data.Repositories
{
    public class AuthRepository(AppDbContext _context) : IAuthRepository
    {
        public async Task<RefreshToken> GetRefereshToken(string refresh)
        {
            return await _context.RefreshTokens
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Token == refresh);
        }
        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
