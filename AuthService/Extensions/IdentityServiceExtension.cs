using AuthAPI.Data;
using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace AuthAPI.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt => { })
               .AddRoles<AppRole>()
               .AddRoleManager<RoleManager<AppRole>>()
               .AddEntityFrameworkStores<AuthContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var publicKey = File.ReadAllText(config["Authorization:PublicKeyPath"]);
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(publicKey.ToCharArray());

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config["Authorization:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),
                        ValidateLifetime = true
                    };
                });

            return services;
        }
    }
}
