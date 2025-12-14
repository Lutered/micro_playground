using AuthAPI.Data.Entities;

namespace AuthAPI.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<RefreshToken> GetRefereshToken(string token);
        public Task AddRefreshToken(RefreshToken refreshToken);
        public Task SaveChangesAsync();
    }
}
