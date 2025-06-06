namespace Yield.Tracker.Domain.Dto.Investment;

public record InvestmentRequestDto
(
    decimal InvestedValue, 
    DateOnly StartDate, 
    DateOnly EndDate
);
