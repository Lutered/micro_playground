using Contracts.AuthApi.Requests;
using Contracts.AuthApi.Responses;

namespace AuthAPI.Intrefaces
{
    public interface IAuthService
    {
        public Task<UserResponse> RegisterAsync(Register registerContract);
        public Task<UserResponse> LoginAsync(Login loginContract);
    }
}
