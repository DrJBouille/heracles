namespace Heracles.Application.Dtos.Todo;

public record PageResults<T>(
    IEnumerable<T> Results,
    int Page,
    int PageSize,
    int TotalCount
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}