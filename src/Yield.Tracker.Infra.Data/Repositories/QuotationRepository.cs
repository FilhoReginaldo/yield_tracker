using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yield.Tracker.Domain.Entities;
using Yield.Tracker.Domain.Repositories;
using Yield.Tracker.Infra.Data.Context;

namespace Yield.Tracker.Infra.Data.Repositories;

public class QuotationRepository(ApplicationDbContext applicationDbContext) : IQuotationRepository
{
    public async Task<List<Quotation>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate) =>
        await applicationDbContext.Quotations
            .AsNoTracking()
            .Where(q => q.Date >= startDate && q.Date < endDate)
            .OrderBy(q => q.Date)
            .ToListAsync();
}
