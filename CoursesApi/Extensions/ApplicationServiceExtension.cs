using CoursesApi.Data;
using CoursesApi.Data.Repositories;
using CoursesApi.Data.Repositories.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Contracts.User.Requests;
using System.Reflection;

namespace CoursesApi.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<CourseContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("coursesdb"));
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IParticipantsRepository, ParticipiantsRepository>();


            services.AddMassTransit(c =>
            {
                //c.AddConsumer<UserCreatedConsumer>();
                c.AddRequestClient<GetUserRequest>();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(config.GetConnectionString("rabbitmqm")));

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            //services.AddStackExchangeRedisCache(options => {
            //    options.Configuration = config.GetConnectionString("redis");
            //});

            //services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
