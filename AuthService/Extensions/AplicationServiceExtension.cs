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
using Serilog;
using Elastic.Serilog.Sinks;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Ingest.Elasticsearch;
using Elastic.Transport;

namespace AuthAPI.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("microservice-db"));   
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();

            services.AddMassTransit(c =>
            {
                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(config.GetConnectionString("rabbitmq"));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddIdentityCore<AppUser>(opt => {})
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<AppDbContext>();

            //Log.Logger = new LoggerConfiguration()
            //  .WriteTo.Elasticsearch(
            //    new[] { new Uri(config["ElasticUrl"]) },
            //    opts =>
            //    {
            //        opts.DataStream = new DataStreamName("logs", "users");
            //        opts.BootstrapMethod = BootstrapMethod.Failure;
            //    },
            //    transport =>
            //    {
            //        //transport.Authentication(new BasicAuthentication("elastic", "py8*B*I=UC5MPi0yutgK "));
            //    })
            //  .CreateLogger();

            return services;
        }
    }
}
