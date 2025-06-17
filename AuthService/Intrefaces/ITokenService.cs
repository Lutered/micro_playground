using AuthAPI.Data.Entities;

namespace AuthAPI.Intrefaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
