using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace Heracles.Api.Middlewares;

public static class RateLimitingExtension
{
  public static IServiceCollection AddRateLimiting(this IServiceCollection services)
  {
    services.AddRateLimiter(options =>
    {
      options.AddPolicy("auth", context =>
      {
        var userId = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 5,
          Window = TimeSpan.FromMinutes(1),
          QueueLimit = 0
        });
      });

      options.AddPolicy("user-read", context =>
      {
        var userId = context.User.FindFirst("sub")?.Value;

        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 120,
          Window = TimeSpan.FromMinutes(1),
          QueueLimit = 0
        });
      });

      options.AddPolicy("user-write", context =>
      {
        var userId = context.User.FindFirst("sub")?.Value;

        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 30,
          Window = TimeSpan.FromMinutes(1),
          QueueLimit = 0
        });
      });

      options.OnRejected = async (context, cancellationToken) =>
      {
        var httpContext = context.HttpContext;

        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
          Status = StatusCodes.Status429TooManyRequests,
          Title = "Too Many Requests",
          Detail = "Too many requests. Please try again later.",
          Type = "https://tools.ietf.org/html/rfc9110#section-15.5.20",
          Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add(
          "traceId",
          httpContext.TraceIdentifier
        );

        await httpContext.Response.WriteAsJsonAsync(
          problemDetails,
          cancellationToken
        );
      };
    });
    
    return services;
  }
}