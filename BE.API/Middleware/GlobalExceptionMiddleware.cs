using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace BE.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. RequestPath: {RequestPath}", context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError("Cannot set response headers, response has already started for request: {RequestPath}", context.Request.Path);
                return;
            }

            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Validation Error";
                    response.Detail = argEx.Message;
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Title = "Resource Not Found";
                    response.Detail = "The requested resource was not found";
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Title = "Unauthorized";
                    response.Detail = "You are not authorized to access this resource";
                    break;

                case InvalidOperationException invOpEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Title = "Business Rule Violation";
                    response.Detail = _environment.IsDevelopment() ? invOpEx.Message : "A business rule was violated";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Title = "Internal Server Error";
                    response.Detail = _environment.IsDevelopment() 
                        ? exception.Message 
                        : "An internal server error occurred";
                    break;
            }

            response.TraceId = context.TraceIdentifier;

            if (_environment.IsDevelopment())
            {
                response.StackTrace = exception.StackTrace;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = response.StatusCode;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (!context.Response.HasStarted)
            {
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
    }
}
