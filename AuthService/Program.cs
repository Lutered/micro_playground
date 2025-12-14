using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Shared.Extensions;
using Shared.Middlewares;
using AuthAPI.Settings;

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

var userManager = services.GetRequiredService<UserManager<AppUser>>();
var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
await Seed.SeedRoles(userManager, roleManager);

app.UseAuthorization();

app.MapEndpoints();

app.Run();
