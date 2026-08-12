using Heracles.Application.Dtos.Todo;

namespace Heracles.Application.Services;

public interface ITodoService
{
    Task<TodoDto> CreateTodo(CreateTodoRequestDto request, int ownerId, CancellationToken cancellationToken);
    Task<PageResults<TodoDto>> GetAllTodos(TodoQueryDto query, int ownerId, CancellationToken cancellationToken);
    Task DeleteTodo(int id, CancellationToken cancellationToken);
    Task<TodoDto> GetTodoById(int id, int ownerId, CancellationToken cancellationToken);
    Task<TodoDto> UpdateTodo(int id, UpdateTodoRequestDto request, int ownerId, CancellationToken cancellationToken);
    Task<TodoDto> ToggleTodo(int id, int ownerId, CancellationToken cancellationToken);
}