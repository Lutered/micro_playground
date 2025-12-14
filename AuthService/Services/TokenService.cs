using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.Data.Repositories.Interfaces;
using AuthAPI.Services.Interfaces;
using AuthAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly RsaSecurityKey _key;
        private readonly AuthSettings _authSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthRepository _authRepo;

        public TokenService(
            IAuthRepository authRepo,
            IOptions<AuthSettings> authSettings,
            UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
            this._authSettings = authSettings.Value;
            this._authRepo = authRepo;

            var privateKey = File.ReadAllText(this._authSettings.SecretKeyPath);
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey.ToCharArray());

            _key = new RsaSecurityKey(rsa);
        }

        public async Task<string> GenerateAccessToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("Age", user.Age.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            var creds = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256);

            var configExpireDays = _authSettings.AuthTokenExpireDays;
            var expireDay = !string.IsNullOrEmpty(configExpireDays) ? Int32.Parse(configExpireDays) : 2;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(expireDay),
                Issuer = _authSettings.Issuer,
                Audience = _authSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken(AppUser user, CancellationToken cancellationToken = default)
        {
            var configExpireDays = _authSettings.RefreshTokenExpireDays;
            var expireDay = !string.IsNullOrEmpty(configExpireDays) ? Int32.Parse(configExpireDays) : 7;

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(expireDay)
            };

            _authRepo.AddRefreshToken(refreshToken);
            await _authRepo.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}
