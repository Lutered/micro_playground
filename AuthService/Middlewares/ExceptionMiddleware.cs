using System.Net;

namespace AuthAPI.Middlewares
{
    public class ExceptionMiddleware(
        RequestDelegate _next,
        ILogger<ExceptionMiddleware> _logger)
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
