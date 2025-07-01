using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Consumers;
using UsersAPI.Data;
using UsersAPI.Interfaces;

namespace UsersAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("UserDBConnection"));
            });

            var rabbitMqConfig = config.GetRequiredSection("RabbitMq");
            services.AddMassTransit(c =>
            {

                c.AddConsumer<UserCreatedConsumer>();

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

            var redisConfig = config.GetSection("Redis");
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = redisConfig["Configuration"];
                options.InstanceName = redisConfig["InstanceName"];
            });

            services.AddScoped<UserRepository>();
            services.AddScoped<ICacheService, ICacheService>();

            return services;
        }
    }
}
