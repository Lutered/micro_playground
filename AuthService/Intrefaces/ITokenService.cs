using AuthAPI.Data.Entities;

namespace AuthAPI.Intrefaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        Task<string> GenerateRefreshToken(AppUser user);
    }
}
