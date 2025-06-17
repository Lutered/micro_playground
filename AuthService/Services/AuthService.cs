using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using AutoMapper;
using Contracts;
using Contracts.AuthApi.Requests;
using Contracts.AuthApi.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UserResponse> RegisterAsync(Register registerContract)
        {
            if (await UserExists(registerContract.Username))
                return ContractHelpers.ReturnError<UserResponse>(400, $"User with name {registerContract.Username} already exists");

            var user = _mapper.Map<AppUser>(registerContract);

            user.UserName = registerContract.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerContract.Password);

            if (!result.Succeeded)
                return ContractHelpers.ReturnError<UserResponse>(500, $"Something went wrong during creating user");

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) 
                return ContractHelpers.ReturnError<UserResponse>(500, $"Something went wrong during creating user");

            return new UserResponse()
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        public async Task<UserResponse> LoginAsync(Login loginContract)
        {
            var user = await _userManager.Users
               .FirstOrDefaultAsync(x => x.UserName == loginContract.UserName);

            if (user == null) return ContractHelpers.ReturnError<UserResponse>(401, "Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginContract.Password);

            if (!result) return ContractHelpers.ReturnError<UserResponse>(401, "Invalid password");

            return new UserResponse()
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
