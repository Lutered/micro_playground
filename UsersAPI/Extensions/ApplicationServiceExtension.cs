using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UsersAPI.Consumers;
using UsersAPI.Data;
using UsersAPI.Interfaces;
using UsersAPI.Interfaces.Repositories;
using UsersAPI.Services;
using Serilog;
using Elastic.Serilog.Sinks;
using Elastic.Ingest.Elasticsearch;
using Elastic.Transport;
using Elastic.Ingest.Elasticsearch.DataStreams;

namespace UsersAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            services.AddScoped<IUserRepository, UserRepository>();

            // Настройка Serilog
            var elasticConfig = config.GetSection("Elasticsearch");
            //Log.Logger = new LoggerConfiguration()
            //  .WriteTo.Elasticsearch(
            //    new[] { new Uri(elasticConfig["Url"]) },
            //    opts =>
            //    {
            //        opts.DataStream = new DataStreamName("logs", "users");
            //        opts.BootstrapMethod = BootstrapMethod.Failure;
            //    }, 
            //    transport =>
            //    {
            //      //transport.CertificateFingerprint("xxx");
            //      transport.Authentication(new BasicAuthentication("elastic", "py8*B*I=UC5MPi0yutgK "));
            //    })
            //  .CreateLogger();

            return services;
        }
    }
}
