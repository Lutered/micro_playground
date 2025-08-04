
using Serilog.Sinks.Elasticsearch;
using Serilog;
using UsersAPI.Extensions;
using Serilog.Formatting.Elasticsearch;
using System.Runtime.Serialization;
using UsersAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMvcCore();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
              new Uri(builder.Configuration.GetConnectionString("elasticsearch")))
          {
              AutoRegisterTemplate = true,
              IndexFormat = "users-logs-{0:yyyy.MM.dd}",
              Period = TimeSpan.FromMilliseconds(500)
          })
          .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();