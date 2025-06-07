using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Yield.Tracker.Infra.Data.Context;
using Yield.Tracker.Infra.Data.Repositories;
using Yield.Tracker.Test.Shared.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Yield.Tracker.Integration.Test.Repositories;

public class QuotationRepositoryTests : IClassFixture<IntegrationTestFactory>
{
    private readonly ApplicationDbContext _context;
    private readonly QuotationRepository _repository;

    public QuotationRepositoryTests(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _repository = new QuotationRepository(_context);
    }

    [Fact]
    public async Task GetByDateRangeAsync_ReturnsQuotationsWithinDateRange()
    {
        #region Arrange
        _context.Quotations.RemoveRange(_context.Quotations);
        await _context.SaveChangesAsync();

        _context.Quotations.AddRange(QuotationEntityTest.QuotationListDefault());

        await _context.SaveChangesAsync();

        var repository = new QuotationRepository(_context);

        var startDate = new DateOnly(2025, 01, 01);
        var endDate = new DateOnly(2025, 01, 31);

        #endregion

        #region Act
        var result = await repository.GetByDateRangeAsync(startDate, endDate);

        #endregion

        #region Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Select(q => q.Date).Should().OnlyContain(d => d >= startDate && d <= endDate);
        result.Should().BeInAscendingOrder(q => q.Date);

        #endregion
    }
}
