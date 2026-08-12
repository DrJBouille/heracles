using System.Security.Claims;
using Heracles.Api.Filters;
using Heracles.Application.Dtos.Todo;
using Heracles.Application.Services;

namespace Heracles.Api.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").WithTags("Todos").RequireAuthorization();

        group.MapGet("/", GetAll)
            .RequireRateLimiting("user-read")
            .AddEndpointFilter<ValidationFilter<TodoQueryDto>>();
        
        group.MapPost("/", Create)
            .RequireRateLimiting("user-write")
            .AddEndpointFilter<ValidationFilter<CreateTodoRequestDto>>();
        
        group.MapDelete("/{id}", Delete)
            .RequireRateLimiting("user-write")
            .RequireAuthorization("Admin");
        
        group.MapGet("/{id}", GetById)
            .RequireRateLimiting("user-read");
        
        group.MapPut("/{id}", Update)
            .RequireRateLimiting("user-write")
            .AddEndpointFilter<ValidationFilter<UpdateTodoRequestDto>>();
        
        group.MapPatch("/{id}", Toggle)
            .RequireRateLimiting("user-write");
        
        return group;
    }

    static async Task<IResult> GetById(int id, ClaimsPrincipal user, ITodoService service, CancellationToken cancellationToken)
    {
        var ownerId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var todo = await service.GetTodoById(id, ownerId, cancellationToken);
        return Results.Ok(todo);
    }
    
    static async Task<IResult> GetAll([AsParameters] TodoQueryDto query, ClaimsPrincipal user, ITodoService service, CancellationToken cancellationToken)
    {
        var ownerId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var todos = await service.GetAllTodos(query, ownerId, cancellationToken);
        return Results.Ok(todos);
    }

    static async Task<IResult> Create(CreateTodoRequestDto request, ClaimsPrincipal user, ITodoService service, CancellationToken cancellationToken)
    {
        var ownerId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var todo = await service.CreateTodo(request, ownerId, cancellationToken);
        return Results.Created($"/api/todos/{todo.Id}", todo);
    }

    static async Task<IResult> Delete(int id, ITodoService service, CancellationToken cancellationToken)
    {
        await  service.DeleteTodo(id, cancellationToken);
        return Results.NoContent();
    }

    static async Task<IResult> Update(int id, UpdateTodoRequestDto request, ClaimsPrincipal user, ITodoService service, CancellationToken cancellationToken)
    {
        var ownerId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var todo = await service.UpdateTodo(id, request, ownerId, cancellationToken);
        return Results.Ok(todo);
    }

    static async Task<IResult> Toggle(int id, ITodoService service, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var ownerId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var todo = await service.ToggleTodo(id, ownerId, cancellationToken);
        return Results.Ok(todo);
    }
}