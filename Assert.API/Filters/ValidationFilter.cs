using Assert.Application.Exceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assert.API.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var failures = context.ModelState
                .Where(entry => entry.Value.Errors.Count > 0)
                .SelectMany(entry => entry.Value.Errors
                    .Select(error => new ValidationFailure(entry.Key, error.ErrorMessage)))
                .ToList();

            throw new ValidationException(failures);
        }
        await next();
    }

}
