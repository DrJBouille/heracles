namespace Heracles.Application.Dtos.Todo;

public record TodoQueryDto(
    int Page = 1, 
    int PageSize = 20,
    bool? Completed = null,
    string? Search = null,
    string SortBy = "createdAt",
    string SortOrder = "desc"
);