using AuthAPI.Consumers;
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
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();

            //services.AddMassTransit(c => 
            //{
               
            //    c.AddConsumer<RegisterConsumer>();
            //    c.AddConsumer<LoginConsumer>();

            //    c.UsingRabbitMq((ctx, cfg) => 
            //    {
            //        cfg.Host("localhost", "/", h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });

            //        cfg.ReceiveEndpoint("register-event", e =>
            //        {
            //            e.ConfigureConsumer<RegisterConsumer>(ctx);
            //        });

            //        cfg.ReceiveEndpoint("login-event", e =>
            //        {
            //            e.ConfigureConsumer<LoginConsumer>(ctx);
            //        });
            //    });
            //});


            return services;
        }
    }
}
