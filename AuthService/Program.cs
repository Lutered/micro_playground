using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Logging.AddConsole();

var app = builder.Build();

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

var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
await Seed.SeedRoles(roleManager);

app.MapEndpoints();
app.MapControllers();

app.Run();
