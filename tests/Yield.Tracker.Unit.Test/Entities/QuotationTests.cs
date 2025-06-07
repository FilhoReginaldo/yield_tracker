using FluentAssertions;
using Xunit;
using Yield.Tracker.Domain.Entities;
using Yield.Tracker.Domain.Entities.Validator;

namespace Yield.Tracker.Unit.Test.Entities;

public class QuotationTests
{
    [Fact]
    public void Create_ValidQuotation_ShouldSucceed()
    {
        #region Arrange
        int id = 1;
        DateOnly date = new(2025, 1, 10);
        string index = "CDI";
        decimal rate = 13.25m;

        #endregion

        #region Act
        var quotation = new Quotation(id, date, index, rate);

        #endregion

        #region Assert
        quotation.Id.Should().Be(id);
        quotation.Date.Should().Be(date);
        quotation.Index.Should().Be(index);
        quotation.Rate.Should().Be(rate);

        #endregion
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_InvalidIndexNullOrEmpty_ShouldThrow(string invalidIndex)
    {
        #region Act
        Action act = () => new Quotation(1, new DateOnly(2025, 1, 1), invalidIndex, 13.5m);

        #endregion

        #region Assert
        act.Should()
            .Throw<DomainExceptionValidator>()
            .WithMessage("Invalid index. index is required");

        #endregion
    }

    [Fact]
    public void Create_IndexTooLong_ShouldThrow()
    {
        #region Arrange
        string longIndex = new string('A', 31);

        #endregion

        #region Act
        Action act = () => new Quotation(1, new DateOnly(2025, 1, 1), longIndex, 12m);

        #endregion

        #region Assert
        act.Should()
            .Throw<DomainExceptionValidator>()
            .WithMessage("Invalid index, maximum of 30 characters");

        #endregion
    }

    [Fact]
    public void Create_RateLessThanOrEqualZero_ShouldThrow()
    {
        #region Act
        Action actZero = () => new Quotation(1, new DateOnly(2025, 1, 1), "CDI", 0);
        Action actNegative = () => new Quotation(1, new DateOnly(2025, 1, 1), "CDI", -0.01m);

        #endregion

        #region Assert
        actZero.Should().Throw<DomainExceptionValidator>().WithMessage("Invalid rate. Rate must be greater than zero");
        actNegative.Should().Throw<DomainExceptionValidator>().WithMessage("Invalid rate. Rate must be greater than zero");

        #endregion
    }
}
