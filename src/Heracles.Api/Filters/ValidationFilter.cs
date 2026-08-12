using FluentValidation;

namespace Heracles.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        
        if (validator is null) return await next(context);
        
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null) return await next(context);
        
        var validationResult = await validator.ValidateAsync(argument);

        if (validationResult.IsValid) return await next(context);
        
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
            
        return Results.ValidationProblem(errors);
    }
}