using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Shared.Extensions;
using Shared.Middlewares;
using AuthAPI.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices((builder.Configuration));
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddLogs(builder.Configuration, builder.Host);

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Authorization"));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapDefaultEndpoints();
app.UseDeveloperExceptionPage();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

if (!app.Environment.IsProduction())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AuthContext>();
        await db.Database.MigrateAsync();
        logger.LogInformation("Migration has been done successfully");
    }
    catch(Exception ex)
    {
        logger.LogError($"An error occured during migration: {ex}");
        throw;
    }

}

var userManager = services.GetRequiredService<UserManager<AppUser>>();
var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

try
{
    await Seed.SeedData(userManager, roleManager);
    logger.LogInformation("Data seeding has been done successfully");
}
catch(Exception ex)
{
    logger.LogError($"An error occured during data seeding: {ex}");
}

app.UseAuthorization();
app.MapEndpoints();

app.Run();
