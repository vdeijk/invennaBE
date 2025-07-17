using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

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
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case ArgumentException argEx:
                    // Business validation failures
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Validation Error";
                    response.Detail = argEx.Message;
                    break;

                case KeyNotFoundException:
                    // Entity not found
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Title = "Resource Not Found";
                    response.Detail = "The requested resource was not found";
                    break;

                case UnauthorizedAccessException:
                    // Authorization failures
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Title = "Unauthorized";
                    response.Detail = "You are not authorized to access this resource";
                    break;

                case InvalidOperationException invOpEx:
                    // Business rule violations
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Title = "Business Rule Violation";
                    response.Detail = _environment.IsDevelopment() ? invOpEx.Message : "A business rule was violated";
                    break;

                default:
                    // All other exceptions
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Title = "Internal Server Error";
                    response.Detail = _environment.IsDevelopment() 
                        ? exception.Message 
                        : "An internal server error occurred";
                    break;
            }

            // Add trace ID for debugging
            response.TraceId = context.TraceIdentifier;

            // Include stack trace in development
            if (_environment.IsDevelopment())
            {
                response.StackTrace = exception.StackTrace;
            }

            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
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
