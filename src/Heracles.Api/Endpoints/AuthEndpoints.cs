using Heracles.Application.Dtos.Auth;
using Heracles.Application.Services.Auth;

namespace Heracles.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", Register)
            .RequireRateLimiting("auth");
        group.MapPost("/login", Login)
            .RequireRateLimiting("auth");
        
        return group;
    }

    static async Task<IResult> Register(RegisterRequestDto request, IAuthService service)
    {
        var result = await service.RegisterAsync(request);
        return Results.Ok(result);
    }
    
    static async Task<IResult> Login(LoginRequestDto request, IAuthService service)
    {
        var result = await service.LoginAsync(request);
        return Results.Ok(result);
    }
}