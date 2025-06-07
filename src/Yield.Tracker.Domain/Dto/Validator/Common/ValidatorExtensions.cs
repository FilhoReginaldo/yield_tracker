using ErrorOr;
using FluentValidation;
using FluentValidation.Results;

namespace Yield.Tracker.Domain.Dto.Validator.Common;

public static class ValidatorExtensions
{
    public static List<Error>? ValidateToErrors<T>(this IValidator<T> validator, T instance)
    {
        ValidationResult result = validator.Validate(instance);
        if (result.IsValid)
            return null;

        return result.Errors
            .Select(e => Error.Validation(code: e.PropertyName, description: e.ErrorMessage))
            .ToList();
    }
}
