using AuthAPI.Data.Entities;

namespace AuthAPI.Intrefaces
{
    public interface IAuthRepository
    {
        public Task AddRefreshToken(RefreshToken refreshToken);
        public Task SaveChangesAsync();
    }
}
