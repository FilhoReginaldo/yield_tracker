namespace Yield.Tracker.Domain.Dto.Investment;

public record InvestmentResponseDto
(
    decimal AccumulatedFactor,
    decimal UpdatedValue
);
