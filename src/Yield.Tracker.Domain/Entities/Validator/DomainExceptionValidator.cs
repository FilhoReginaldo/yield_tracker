namespace Yield.Tracker.Domain.Entities.Validator;

public class DomainExceptionValidator(string error) : Exception(error)
{
    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new DomainExceptionValidator(error);
    }
}
