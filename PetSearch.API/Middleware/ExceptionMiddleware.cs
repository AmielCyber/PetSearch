using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace PetSearch.API.Middleware;

/// <summary>
/// Our global middleware that will catch errors and log them.
/// </summary>
public class ExceptionMiddleware
{
    private const string DefaultErrorDetail = "An error occurred while processing your request.";
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = 500;

            var response = new ProblemDetails
            {
                Title = e.Message,
                // Only show stack trace in development.
                Detail = GetErrorDetail(e),
                Status = (int)HttpStatusCode.InternalServerError,
            };


            // We are outside our api controller, so we need to specify JsonNamingPolicy
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Gets the error detail from an exception.
    /// </summary>
    /// <param name="exception">Exception object</param>
    /// <returns>Stacktrace string if in development, else the default error message.</returns>
    private string? GetErrorDetail(Exception exception)
    {
        return _env.IsDevelopment() ? exception.StackTrace : DefaultErrorDetail;
    }
}