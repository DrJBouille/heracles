using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Heracles.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = MapException(exception);
        
        logger.LogError(exception, title);

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Type = $"https://tools.ietf.org/html/rfc9110#section-{GetRfcSection(statusCode)}",
            Instance = context.Request.Path,
        };
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
        
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
        
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static (HttpStatusCode StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
        ArgumentException => (HttpStatusCode.BadRequest, "Invalid request."),
        InvalidOperationException => (HttpStatusCode.Conflict, "Impossible operation."),
        _ => (HttpStatusCode.InternalServerError, "Internal server error.")
    };

    private static string GetRfcSection(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.BadRequest => "15.5.1",
        HttpStatusCode.NotFound => "15.5.5",
        HttpStatusCode.Conflict => "15.5.10",
        _ => "15.6.1"
    };
}