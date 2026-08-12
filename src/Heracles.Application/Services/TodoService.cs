using Heracles.Application.Dtos.Todo;
using Heracles.Domain.Entities;
using Heracles.Domain.Interfaces;

namespace Heracles.Application.Services;

public class TodoService(ITodoRepository repository) : ITodoService
{
    public async Task<TodoDto> CreateTodo(CreateTodoRequestDto request, int ownerId, CancellationToken cancellationToken)
    {
        var todo = Todo.Create(request.Title, ownerId);

        await repository.AddAsync(todo);
        await repository.SaveChangesAsync(cancellationToken);

        return MapTodoDto(todo);
    }

    public async Task<PageResults<TodoDto>> GetAllTodos(TodoQueryDto query, int ownerId, CancellationToken cancellationToken)
    {
        var (todos, totalCount) = await repository.GetAllTodos(ownerId, query.Page, query.PageSize, query.Completed, query.Search, query.SortBy, query.SortOrder, cancellationToken);
        
        var items = todos.Select(MapTodoDto).ToList();
        
        return new PageResults<TodoDto>(items, query.Page, query.PageSize, totalCount);
    }

    public async Task DeleteTodo(int id, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Todo not found.");
        
        repository.Remove(todo);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<TodoDto> GetTodoById(int id, int ownerId, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(id, ownerId, cancellationToken) ?? throw new KeyNotFoundException("Todo not found.");
        return MapTodoDto(todo);
    }
    

    public async Task<TodoDto> UpdateTodo(int id, UpdateTodoRequestDto request, int ownerId, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(id, ownerId, cancellationToken) ?? throw new KeyNotFoundException("Todo not found.");
        
        todo.Rename(request.Title);
        await repository.SaveChangesAsync(cancellationToken);

        return MapTodoDto(todo);
    }

    public async Task<TodoDto> ToggleTodo(int id, int ownerId, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(id, ownerId, cancellationToken) ?? throw new KeyNotFoundException("Todo not found.");
        
        todo.ToggleCompleted();
        await repository.SaveChangesAsync(cancellationToken);

        return MapTodoDto(todo);
    }

    private static TodoDto MapTodoDto(Todo todo) => new(
        todo.Id,
        todo.Title,
        todo.IsCompleted,
        todo.OwnerId,
        todo.CreatedAt
    );
}