using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Yield.Tracker.Application.Services;
using Yield.Tracker.Domain.Entities;
using Yield.Tracker.Domain.Repositories;
using Yield.Tracker.Test.Shared.Dto;
using Yield.Tracker.Test.Shared.Entities;

namespace Yield.Tracker.Unit.Test.Services
{
    public class InvestmentCalculatorServiceTests
    {
        private readonly Mock<IQuotationRepository> _quotationRepositoryMock;
        private readonly Mock<ILogger<InvestmentCalculatorService>> _loggerMock;
        private readonly InvestmentCalculatorService _service;

        public InvestmentCalculatorServiceTests()
        {
            _quotationRepositoryMock = new Mock<IQuotationRepository>();
            _loggerMock = new Mock<ILogger<InvestmentCalculatorService>>();
            _service = new InvestmentCalculatorService(_quotationRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CalculateAsync_ReturnsValidationError_WhenInvestedValueIsInvalid()
        {
            #region Arrange
            var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault();

            #endregion

            #region Act
            var result = await _service.CalculateAsync(request);

            #endregion

            #region Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);

            #endregion
        }

        [Fact]
        public async Task CalculateAsync_ReturnsValidationError_WhenNoQuotationsFound()
        {
            #region Arrange
            var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(1000);
            _quotationRepositoryMock
                .Setup(repo => repo.GetByDateRangeAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(new List<Quotation>());

            #endregion

            #region Act
            var result = await _service.CalculateAsync(request);

            #endregion

            #region Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("cotacao.NaoEncontrada");

            #endregion
        }

        [Fact]
        public async Task CalculateAsync_ReturnsUpdatedValue_WhenInputIsValid()
        {
            #region Arrange
            var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(1000);
            var quotations = QuotationEntityTest.QuotationListDefault();
            
            _quotationRepositoryMock
                .Setup(repo => repo.GetByDateRangeAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(quotations);

            #endregion

            #region Act
            var result = await _service.CalculateAsync(request);

            #endregion

            #region Assert
            result.IsError.Should().BeFalse();
            result.Value.UpdatedValue.Should().BeGreaterThan(1000);

            #endregion
        }

        [Fact]
        public async Task CalculateAsync_ReturnsError_WhenPreviousDayQuotationMissing()
        {
            #region Arrange
            var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(1000);
            var quotations = new List<Quotation>
            {
                QuotationEntityTest.QuotationDefault(),
            };

            _quotationRepositoryMock
                .Setup(repo => repo.GetByDateRangeAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(quotations);

            #endregion

            #region Act
            var result = await _service.CalculateAsync(request);

            #endregion

            #region Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Cotação para o dia anterior 02/01/2025 não encontrada.");

            #endregion
        }
    }
}
