using Yield.Tracker.Domain.Entities.Validator;

namespace Yield.Tracker.Domain.Entities;

public class Quotation
{
    public Quotation(int id, DateOnly date, string index, decimal rate)
    {
        Id = id;
        Date = date;
        ValidateDomain(index, rate);
    }

    public int Id { get; private set; }
    public DateOnly Date { get; private set; }
    public string Index { get; private set; } = null!;
    public decimal Rate { get; private set; }

    #region Validate
    private void ValidateDomain(string index, decimal rate)
    {
        //Validator index
        DomainExceptionValidator.When(string.IsNullOrEmpty(index), "Invalid index. index is required");
        DomainExceptionValidator.When(index.Length < 0, "Invalid index, minimum of 1 characters");
        DomainExceptionValidator.When(index.Length > 30, "Invalid index, maximum of 30 characters");

        // Validator rate
        DomainExceptionValidator.When(rate <= 0, "Invalid rate. Rate must be greater than zero");

        Index = index;
        Rate = rate;
    }

    #endregion
}
