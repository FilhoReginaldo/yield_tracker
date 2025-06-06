namespace Yield.Tracker.Domain.Entities;

public class Quotation
{
    public int Id { get; private set; }
    public DateOnly Date { get; private set; }
    public string Index { get; private set; } = null!;
    public decimal Rate { get; private set; }
}
