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
            .InclusiveBetween(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31))
            .WithMessage("A data de início deve estar entre 01/01/2025 e 31/01/2025.");

        RuleFor(x => x.EndDate)
            .InclusiveBetween(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31))
            .WithMessage("A data final deve estar entre 01/01/2025 e 31/01/2025.");

        RuleFor(x => x)
            .Must(x => x.StartDate <= x.EndDate)
            .WithMessage("A data de início deve ser menor ou igual à data final.");
    }
}
