using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;

namespace AuthAPI.Data.Repositories
{
    public class AuthRepository(AppDbContext _context) : IAuthRepository
    {
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
