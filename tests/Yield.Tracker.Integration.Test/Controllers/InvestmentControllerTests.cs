using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Yield.Tracker.Test.Shared.Dto;

namespace Yield.Tracker.Integration.Test.Controllers;

public class InvestmentControllerTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;

    public InvestmentControllerTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Calculate_ReturnsBadRequest_WhenValidationFails()
    {
        #region Arrange
        var request = InvestmentRequestDtoTests.InvestmentRequestDtoDefault(0);

        #endregion

        #region Act
        var response = await _client.PostAsJsonAsync("v1/investment/calculate", request);

        #endregion

        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();

        #endregion
    }
}
