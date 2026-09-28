using CommuteMate.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace CommuteMate.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning("Handled error: {Message}", ex.Message);
                await WriteErrorAsync(context, ex.StatusCode, ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unhandled error on {Method}{path}",
                    context.Request.Method,
                    context.Request.Path
                );
                await WriteErrorAsync(context,(int) HttpStatusCode.InternalServerError, "Something went wrong.please try again later.");
            }
        }
        private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
        {
            if(context.Response.HasStarted)
            {
                return;
            }
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var json=JsonSerializer.Serialize(new { error = message }
            ,new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json);
        }
    }
}
