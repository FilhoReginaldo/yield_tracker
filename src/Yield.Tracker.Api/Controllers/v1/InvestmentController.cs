using Microsoft.AspNetCore.Mvc;
using Yield.Tracker.Domain.Dto.Investment;
using Yield.Tracker.Domain.Services;

namespace Yield.Tracker.Api.Controllers.v1;

[Route("v1/investment")]
public class InvestmentController(IInvestmentCalculatorService investmentCalculatorService) : ApiControllerBase
{
    [HttpPost("calculate")]
    public async Task<ActionResult<InvestmentResponseDto>> Calculate([FromBody]InvestmentRequestDto investmentRequest) =>
        FromErrorOr(await investmentCalculatorService.CalculateAsync(investmentRequest));
}
