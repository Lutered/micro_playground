using AuthAPI.Data.Entities;

namespace AuthAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        Task<string> GenerateRefreshToken(AppUser user);
    }
}
