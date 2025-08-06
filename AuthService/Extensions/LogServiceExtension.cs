using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace AuthAPI.Extensions
{
    public static class LogServiceExtension
    {
        public static IServiceCollection AddLogs(this IServiceCollection services, IConfiguration config, IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                  new Uri(config.GetConnectionString("elasticsearch")))
              {
                  AutoRegisterTemplate = true,
                  IndexFormat = "auth-logs-{0:yyyy.MM.dd}",
                  Period = TimeSpan.FromMilliseconds(500)
              })
              .CreateLogger();

            host.UseSerilog();

            return services;
        }
    }
}
