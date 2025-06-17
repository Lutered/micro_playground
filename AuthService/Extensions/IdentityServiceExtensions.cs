using AuthAPI.Data;
using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAPI.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                //opt.User.RequireUniqueEmail = true;
            })
           .AddRoles<AppRole>()
           .AddRoleManager<RoleManager<AppRole>>()
           .AddEntityFrameworkStores<AppDbContext>();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        string key = config["TokenKey"] ?? "";
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });

            return services;
        }
    }
}
