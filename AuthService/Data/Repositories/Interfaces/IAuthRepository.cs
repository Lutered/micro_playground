using AuthAPI.Data.Entities;

namespace AuthAPI.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<RefreshToken> GetRefereshToken(string token, CancellationToken cancellationToken = default);
        public void AddRefreshToken(RefreshToken refreshToken);
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
