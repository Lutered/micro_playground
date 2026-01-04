using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using UsersAPI.Data;
using System.Reflection;
using UsersAPI.Data.Repositories;
using UsersAPI.Data.Repositories.Interfaces;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using UsersAPI.Helpers;
using UsersAPI.Consumers;
using Shared.Models.Contracts.User.Requests.DeleteUser;

namespace UsersAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers()
               .AddNewtonsoftJson();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { 
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); 
            });
            //services.AddSwaggerGenNewtonsoftSupport();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<UserContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("usersdb"));
            });

            services.AddMassTransit(c =>
            {
                c.AddConsumer<CreateUserConsumer>();
                c.AddConsumer<GetUserById>();

                c.AddRequestClient<DeleteUserRequest>();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(config.GetConnectionString("rabbitmqm")));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = config.GetConnectionString("redis");
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserCacheHelper>();

            return services;
        }
    }
}
