using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yield.Tracker.Application.Services;
using Yield.Tracker.Domain.Repositories;
using Yield.Tracker.Domain.Services;
using Yield.Tracker.Infra.Data.Context;
using Yield.Tracker.Infra.Data.Repositories;

namespace Yield.Tracker.Infra.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Conection
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("DefaultConnection not found");
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<ApplicationDbContext>(options =>
         options.UseNpgsql(connectionString));

        //Repository
        services.AddScoped<IQuotationRepository, QuotationRepository>();

        //Service
        services.AddScoped<IInvestmentCalculatorService, InvestmentCalculatorService>();

        return services;
    }
}
