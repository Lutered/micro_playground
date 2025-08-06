using AuthAPI.Data.Entities;

namespace AuthAPI.Intrefaces
{
    public interface IAuthRepository
    {
        public Task<RefreshToken> GetRefereshToken(string token);
        public Task AddRefreshToken(RefreshToken refreshToken);
        public Task SaveChangesAsync();
    }
}
