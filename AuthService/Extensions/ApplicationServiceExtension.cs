using AuthAPI.Data;
using AuthAPI.Intrefaces;
using AuthAPI.Sagas;
using AuthAPI.Sagas.Instances;
using AuthAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MassTransit.EntityFrameworkCoreIntegration;
using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using AuthAPI.Consumers;
using System.Reflection;
using AuthAPI.Data.Repositories;

namespace AuthAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //services.AddControllers();

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("authdb"));   
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddMassTransit(c =>
            {
                //c.AddRequestClient<UserCreated>();
                c.AddConsumer<UserDeletedConsumer>();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(config.GetConnectionString("rabbitmqm")));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
