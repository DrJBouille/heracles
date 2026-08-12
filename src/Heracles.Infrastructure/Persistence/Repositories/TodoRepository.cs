using Heracles.Domain.Entities;
using Heracles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Heracles.Infrastructure.Persistence.Repositories;

public class TodoRepository(HeraclesDbContext context) : ITodoRepository
{
    public async Task<Todo?> GetByIdAsync(int id, int ownerId, CancellationToken cancellationToken) => 
        await context.Todos.AsNoTracking().FirstOrDefaultAsync(todo => todo.Id == id && todo.OwnerId == ownerId, cancellationToken);
    
    public async Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken) => 
        await context.Todos.AsNoTracking().FirstOrDefaultAsync(todo => todo.Id == id, cancellationToken);
    
    public async Task<(IEnumerable<Todo> Items, int TotalCount)> GetAllTodos(int ownerId, int page, int pageSize, bool? isCompleted, string? search, string sortBy, string sortOrder, CancellationToken cancellationToken)
    {
        IQueryable<Todo> query = context.Todos;
        
        query = query.Where(todo => todo.OwnerId == ownerId);

        if (isCompleted.HasValue)
        {
            query = query.Where(todo => todo.IsCompleted == isCompleted.Value);
        }

        if (string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(todo => EF.Functions.ILike(todo.Title, $"%{search}%"));
        }

        query = sortBy.ToLower() switch
        {
            "title" => sortOrder == "desc"
                ? query.OrderByDescending(todo => todo.Title)
                : query.OrderBy(todo => todo.Title),
            
            "createdat" => sortOrder == "desc"
                ? query.OrderByDescending(todo => todo.CreatedAt)
                : query.OrderBy(todo => todo.CreatedAt),
            
            _ => sortOrder == "desc"
                ? query.OrderByDescending(todo => todo.CreatedAt)
                : query.OrderBy(todo => todo.CreatedAt),
        };
        
        var totalCount = await query.CountAsync();
        
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync(cancellationToken);
        
        return (items,  totalCount);
    }
    
    public void Remove(Todo todo) => context.Todos.Remove(todo);
    
    public async Task AddAsync(Todo todo) => await context.Todos.AddAsync(todo);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)  => await context.SaveChangesAsync(cancellationToken);
}