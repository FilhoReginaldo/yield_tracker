using Yield.Tracker.Domain.Dto.Investment;

namespace Yield.Tracker.Test.Shared.Dto;

public static class InvestmentResponseDtoTests
{
    public static InvestmentResponseDto InvestmentResponseDtoDefault() =>
        new InvestmentResponseDto(1.02m, 1020m);
}
