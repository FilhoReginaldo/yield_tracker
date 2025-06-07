using Yield.Tracker.Domain.Entities;

namespace Yield.Tracker.Test.Shared.Entities;

public static class QuotationEntityTest
{
    public static Quotation QuotationDefault() =>
        new Quotation(1, new DateOnly(2025, 1, 1), "CDI", 12.5m);

    public static List<Quotation> QuotationListDefault() =>
        new List<Quotation>
        {
            new Quotation(1, new DateOnly(2025, 1, 1), "CDI", 12.5m),
            new Quotation(2, new DateOnly(2025, 1, 2), "CDI", 12.0m),
            new Quotation(3, new DateOnly(2025, 1, 3), "CDI", 13.5m),
            new Quotation(4, new DateOnly(2025, 1, 4), "CDI", 14.2m),
            new Quotation(5, new DateOnly(2025, 1, 6), "CDI", 14.2m),
        };
}
