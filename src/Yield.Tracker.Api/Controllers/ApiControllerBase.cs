using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Yield.Tracker.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> FromErrorOr<T>(ErrorOr<T> result)
    {
        return result.Match(
            value => Ok(value),
            errors => Problem(errors));
    }

    protected ActionResult Problem(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            return Problem();
        }

        var firstError = errors[0];
        var statusCode = MapErrorToStatusCode(firstError);

        var problemDetails = new ProblemDetails
        {
            Title = firstError.Description,
            Status = statusCode,
            Detail = firstError.Description,
            Instance = HttpContext?.Request?.Path
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private int MapErrorToStatusCode(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
