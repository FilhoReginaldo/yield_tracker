using Yield.Tracker.Domain.Entities;

namespace Yield.Tracker.Domain.Repositories;

public interface IQuotationRepository
{
    Task<List<Quotation>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}
