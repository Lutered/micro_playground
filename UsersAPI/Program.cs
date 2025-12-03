
using UsersAPI.Extensions;
using System.Reflection;
using Shared.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMvcCore();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddLogs(builder.Configuration, builder.Host);

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

app.MapEndpoints();
app.MapControllers();

app.Run();