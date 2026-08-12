using FluentValidation;

namespace Heracles.Application.Dtos.Todo.Validator;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequestDto>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(128)
            .WithMessage("Title must not exceed 128 characters.");
    }
}