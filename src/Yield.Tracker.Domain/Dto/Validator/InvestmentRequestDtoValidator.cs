using FluentValidation;
using Yield.Tracker.Domain.Dto.Investment;

namespace Yield.Tracker.Domain.Dto.Validator;

public class InvestmentRequestDtoValidator : AbstractValidator<InvestmentRequestDto>
{
    public InvestmentRequestDtoValidator()
    {
        RuleFor(x => x.InvestedValue)
            .GreaterThan(0)
            .WithMessage("O valor investido deve ser maior que zero.");

        RuleFor(x => x.StartDate)
             .LessThan(x => x.EndDate)
            .WithMessage("O campo startDate deve ser anterior ao endDate.");
    }
}
