
using UsersAPI.Extensions;
using System.Reflection;
using Shared.Extensions;
using Shared.Middlewares;
using UsersAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMvcCore();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddLogs(builder.Configuration, builder.Host);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();


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

if (!app.Environment.IsProduction())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<UserContext>();
        await db.Database.MigrateAsync();
        logger.LogInformation("Migration has been done successfully");
    }
    catch (Exception ex)
    {
        logger.LogError($"An error occured during migration: {ex}");
        throw;
    }

}

app.MapEndpoints();
app.MapControllers();

app.Run();