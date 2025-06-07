using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Yield.Tracker.Api.Controllers.v1;
using Yield.Tracker.Domain.Dto.Investment;
using Yield.Tracker.Domain.Services;
using Yield.Tracker.Test.Shared.Dto;

namespace Yield.Tracker.Unit.Test.Controller;

public class InvestmentControllerTests
{
    [Fact]
    public async Task Calculate_ReturnsOk_WhenCalculationSucceeds()
    {
        #region Arrange
        var mockService = new Mock<IInvestmentCalculatorService>();
        var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(10);
        var response = InvestmentResponseDtoTests.InvestmentResponseDtoDefault();

        mockService
            .Setup(s => s.CalculateAsync(request))
            .ReturnsAsync(ErrorOrFactory.From(response));

        var controller = new InvestmentController(mockService.Object);

        #endregion

        #region Act
        var result = await controller.Calculate(request);

        #endregion

        #region Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actual = okResult.Value as InvestmentResponseDto;
        actual.Should().BeEquivalentTo(response);

        #endregion
    }

    [Fact]
    public async Task Calculate_ReturnsBadRequest_WhenCalculationFails()
    {
        #region Arrange
        var mockService = new Mock<IInvestmentCalculatorService>();
        var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(); 
        var error = Error.Validation("InvestedValue", "O valor investido deve ser maior que zero.");

        mockService
            .Setup(s => s.CalculateAsync(request))
            .ReturnsAsync(error);

        var controller = new InvestmentController(mockService.Object);

        #endregion

        #region Act
        var result = await controller.Calculate(request);

        #endregion

        #region Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(400, objectResult.StatusCode);

        #endregion
    }
}
