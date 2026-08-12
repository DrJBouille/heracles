using Heracles.Domain.Entities;

namespace Heracles.Domain.Interfaces;

public interface ITodoRepository
{
    Task<Todo?>  GetByIdAsync(int id, int ownerId, CancellationToken cancellationToken);
    Task<Todo?>  GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(IEnumerable<Todo> Items, int TotalCount)> GetAllTodos(
        int ownerId, 
        int page, 
        int pageSize, 
        bool? isCompleted, 
        string? search, 
        string sortBy, 
        string sortOrder, 
        CancellationToken cancellationToken
    );
    void Remove(Todo todo);
    Task AddAsync(Todo todo);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}