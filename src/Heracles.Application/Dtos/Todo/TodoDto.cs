namespace Heracles.Application.Dtos.Todo;

public record TodoDto(
    int Id,
    string Title,
    bool IsCompleted,
    int OwnerId,
    DateTime CreatedAt
);