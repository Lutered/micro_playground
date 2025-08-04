using System.Net;
using System.Text.Json;

namespace UsersAPI.Middlewares
{
    public class ExceptionMiddleware(
        RequestDelegate _next, 
        ILogger<ExceptionMiddleware> _logger,
        IHostEnvironment _env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync("Internal server error");
            }
        }
    }
}
