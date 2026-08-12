using FluentValidation;

namespace Heracles.Application.Dtos.Todo.Validator;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequestDto>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(128)
            .WithMessage("Title cannot exceed 128 characters.");
    }
}