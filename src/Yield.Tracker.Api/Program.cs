using Yield.Tracker.Infra.Ioc.Middleware;
using Yield.Tracker.Infra.Ioc;

namespace Yield.Tracker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllersWithViews();

        builder.Services.AddInfrastructureSwagger();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(x => x.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());

        app.MapControllers();
        app.Run();
    }
}
