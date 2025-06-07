using Yield.Tracker.Domain.Dto.Investment;

namespace Yield.Tracker.Test.Shared.Dto;

public static class InvestmentRequestDtoTests
{
    public static InvestmentRequestDto InvestmentRequestDtoDefault(decimal InvestedValue = 0) =>
        new InvestmentRequestDto(InvestedValue, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 6));
}
