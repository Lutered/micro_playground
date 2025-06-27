using AuthAPI.Data;
using AuthAPI.Intrefaces;
using AuthAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
               // opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                opt.UseNpgsql(config.GetConnectionString("AuthDBConnection"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();

            var rabbitMqConfig = config.GetRequiredSection("RabbitMq");

            services.AddMassTransit(c =>
            {
                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitMqConfig["Host"], rabbitMqConfig["VirtualHost"], h =>
                    {
                        h.Username(rabbitMqConfig["User"]);
                        h.Password(rabbitMqConfig["Password"]);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });


            return services;
        }
    }
}
