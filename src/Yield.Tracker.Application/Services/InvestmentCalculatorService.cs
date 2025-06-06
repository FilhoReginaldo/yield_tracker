using ErrorOr;
using Yield.Tracker.Domain.Dto.Investment;
using Yield.Tracker.Domain.Dto.Validator;
using Yield.Tracker.Domain.Repositories;
using Yield.Tracker.Domain.Services;

namespace Yield.Tracker.Application.Services;

public class InvestmentCalculatorService(IQuotationRepository quotationRepository) : IInvestmentCalculatorService
{
    public async Task<ErrorOr<InvestmentResponseDto>> CalculateAsync(InvestmentRequestDto investmentRequest)
    {
        var validation = new InvestmentRequestDtoValidator().Validate(investmentRequest);
        if(!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();

            return errors;
        }

        var quotations = await quotationRepository.GetByDateRangeAsync(
            investmentRequest.StartDate.AddDays(1),
            investmentRequest.EndDate
        );

        if (quotations == null || !quotations.Any())
            return Error.Validation("cotacao.NaoEncontrada", "Não foram encontradas cotações no período informado.");

        decimal accumulatedFactor = 1.0m;

        foreach (var q in quotations)
        {
            var dailyFactor = Math.Pow(1 + (double)(q.Rate / 100m), 1.0 / 252.0);
            var roundedDailyFactor = Math.Round(dailyFactor, 8, MidpointRounding.AwayFromZero);

            accumulatedFactor *= (decimal)roundedDailyFactor;
        }

        accumulatedFactor = Math.Truncate(accumulatedFactor * 1_0000_0000_0000_0000m) / 1_0000_0000_0000_0000m;

        var updatedValue = Math.Truncate(investmentRequest.InvestedValue * accumulatedFactor * 100_000_000m) / 100_000_000m;

        return new InvestmentResponseDto(accumulatedFactor, updatedValue);
    }
}
