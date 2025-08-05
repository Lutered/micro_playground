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
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("authdb"));   
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();

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

            services.AddIdentityCore<AppUser>(opt => {})
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }
    }
}
