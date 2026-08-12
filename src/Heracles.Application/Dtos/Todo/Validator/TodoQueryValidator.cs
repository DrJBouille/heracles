using FluentValidation;

namespace Heracles.Application.Dtos.Todo.Validator;

public class TodoQueryValidator : AbstractValidator<TodoQueryDto>
{
    public TodoQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortOrder)
            .Must(x =>
                x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("desc", StringComparison.OrdinalIgnoreCase)
            );
        
        RuleFor(x => x.SortBy)
            .Must(BeValidSortProperty)
            .WithMessage("SortBy must be 'title' or 'createdAt'.");
    }

    private static bool BeValidSortProperty(string value)
    {
        return value.Equals("title", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("createdAt", StringComparison.OrdinalIgnoreCase);
    }
}