using ErrorOr;
using Yield.Tracker.Domain.Dto.Investment;

namespace Yield.Tracker.Domain.Services;

public interface IInvestmentCalculatorService
{
    Task<ErrorOr<InvestmentResponseDto>> CalculateAsync(InvestmentRequestDto investmentRequest);
}
