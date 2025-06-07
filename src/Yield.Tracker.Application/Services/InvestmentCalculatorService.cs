using ErrorOr;
using Yield.Tracker.Domain.Dto.Investment;
using Yield.Tracker.Domain.Dto.Validator;
using Yield.Tracker.Domain.Repositories;
using Yield.Tracker.Domain.Services;
using Yield.Tracker.Domain.Dto.Validator.Common;
using Microsoft.Extensions.Logging;

namespace Yield.Tracker.Application.Services;

public class InvestmentCalculatorService(IQuotationRepository quotationRepository, ILogger<InvestmentCalculatorService> logger) : IInvestmentCalculatorService
{
    public async Task<ErrorOr<InvestmentResponseDto>> CalculateAsync(InvestmentRequestDto investmentRequest)
    {
        logger.LogInformation("Iniciando cálculo de investimento para: {@InvestmentRequest}", investmentRequest);

        //Validando o objeto recebido na requisição.
        var validation = new InvestmentRequestDtoValidator().ValidateToErrors(investmentRequest);
        if (validation is not null)
        {
            logger.LogError("Validação falhou para a requisição: {Errors}", validation);
            return validation;
        }
            
        // Obter todas as cotações necessárias (incluindo a do dia anterior ao período de cálculo)
        var allQuotations = await quotationRepository.GetByDateRangeAsync(
            investmentRequest.StartDate.AddDays(-1),
            investmentRequest.EndDate
        );

        if (allQuotations == null || !allQuotations.Any())
        {
            logger.LogError("Nenhuma cotação encontrada para o período de {StartDate} a {EndDate}", investmentRequest.StartDate, investmentRequest.EndDate);
            return Error.Validation("cotacao.NaoEncontrada", "Não foram encontradas cotações no período informado.");
        }
            
        // Ordenar as cotações por data, para garantir um calculo correto
        var orderedQuotations = allQuotations.OrderBy(q => q.Date).ToList();

        decimal accumulatedFactor = 1.0m;
        decimal updatedValue = investmentRequest.InvestedValue;
        var currentDate = investmentRequest.StartDate.AddDays(1);

        while (currentDate < investmentRequest.EndDate)
        {
            //Pular finais de semana
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                currentDate = currentDate.AddDays(1);
                continue;
            }

            // Encontrar a cotação do dia útil anterior
            var previousBusinessDay = GetPreviousBusinessDay(currentDate);
            var previousQuotation = orderedQuotations.FirstOrDefault(q => q.Date == previousBusinessDay);

            if (previousQuotation == null)
            {
                logger.LogError("Cotação não encontrada para o dia anterior {PreviousBusinessDay}", previousBusinessDay);
                return Error.Validation("cotacao.DiaAnteriorNaoEncontrada", $"Cotação para o dia anterior {previousBusinessDay} não encontrada.");
            }
                
            // Calcular fator diário
            double dailyRate = (double)previousQuotation.Rate / 100.0;
            double dailyFactor = Math.Pow(1 + dailyRate, 1.0 / 252.0);
            decimal roundedDailyFactor = Math.Round((decimal)dailyFactor, 8, MidpointRounding.AwayFromZero);

            // Acumular o fator
            accumulatedFactor *= roundedDailyFactor;

            currentDate = currentDate.AddDays(1);
        }

        // Calcular valor atualizado e truncar para 8 casas decimais
        updatedValue = TruncateDecimal(investmentRequest.InvestedValue * accumulatedFactor, 8);

        logger.LogInformation("Fim do cálculo de investimento para: {@InvestmentRequest}", investmentRequest);

        return new InvestmentResponseDto(accumulatedFactor, updatedValue);
    }

    private DateOnly GetPreviousBusinessDay(DateOnly date)
    {
        var previousDay = date.AddDays(-1);
        while (previousDay.DayOfWeek == DayOfWeek.Saturday || previousDay.DayOfWeek == DayOfWeek.Sunday)
        {
            previousDay = previousDay.AddDays(-1);
        }
        return previousDay;
    }

    private decimal TruncateDecimal(decimal value, int decimalPlaces)
    {
        decimal factor = (decimal)Math.Pow(10, decimalPlaces);
        return Math.Truncate(value * factor) / factor;
    }
}