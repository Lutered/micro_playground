using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
    public class TokenService : ITokenService
    {
        //private readonly SymmetricSecurityKey _key;
        private readonly RsaSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            var privateKey = File.ReadAllText(config["TokenPath"]);
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey.ToCharArray());

            _key = new RsaSecurityKey(rsa);
           // _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("Age", user.Age.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            var creds = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256); //SecurityAlgorithms.HmacSha512Signature

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(7),
                Issuer = "AuthAPI",
                Audience = "UserAPI"
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
