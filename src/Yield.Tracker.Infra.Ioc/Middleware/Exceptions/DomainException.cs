using ErrorOr;

namespace Yield.Tracker.Infra.Ioc.Middleware.Exceptions;

public class DomainException : Exception
{
    public List<Error> Errors { get; }

    public DomainException(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }

    public DomainException(Error singleError)
    {
        Errors = new List<Error> { singleError };
    }

    public Error FirstError => Errors.First();
}
