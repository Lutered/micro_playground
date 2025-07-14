using MassTransit;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Consumers;
using UsersAPI.Data;
using UsersAPI.Interfaces;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("usersdb"));
            });

            services.AddMassTransit(c =>
            {
                c.AddConsumer<UserCreatedConsumer>();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config.GetConnectionString("rabbitmq"));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            var redisConnectionStr = config.GetConnectionString("redis");
            var redisConfig = config.GetSection("Redis");
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = config.GetConnectionString("redis");
            });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
