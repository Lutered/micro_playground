using Contracts;
using MassTransit;
using UsersAPI.Consumers;

namespace UsersAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(c =>
            {

                //c.AddConsumer<RegisterConsumer>();
                c.AddConsumer<UserCreatedConsumer>();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);

                    //cfg.ReceiveEndpoint("register-event", e =>
                    //{
                    //    e.ConfigureConsumer<RegisterConsumer>(ctx);
                    //});

                    //cfg.ReceiveEndpoint("login-event", e =>
                    //{
                    //    e.ConfigureConsumer<LoginConsumer>(ctx);
                    //});
                });
            });

            return services;
        }
    }
}
