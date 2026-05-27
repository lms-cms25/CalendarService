using CalendarService.Application.Interfaces;
using CalendarService.Application.Interfaces.External;
using CalendarService.Infrastructure.Persistence.Contexts;
using CalendarService.Infrastructure.Persistence.Repositories;
using CalendarService.Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 

namespace CalendarService.Infrastructure.Extensions.Persistence;

public static class PersistenceServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment) 
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            if (environment.IsDevelopment())
            {
                // 💡 Vid lokal utveckling hämtar vi "DefaultConnection" (din lokala SQL-server eller Docker)
                options.UseInMemoryDatabase("CalendarDevDb");

                // Tips: Om du ABSOLUT vill köra InMemory lokalt kan du göra det, men lokal SQL Server rekommenderas:
                // options.UseInMemoryDatabase("CalendarDevDb");
            }
            else
            {
                // 💡 När appen är deployad hämtar vi "SqlConnection" eller den sträng som din webbserver tillhandahåller
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            }
        });

        services.AddHttpClient<ICourseServiceClient, CourseServiceClient>(client =>
        {
            var baseUrl = configuration["ExternalServices:CourseServiceUrl"]
                ?? throw new InvalidOperationException("Konfigurationen 'ExternalServices:CourseServiceUrl' saknas i dina inställningar.");

            client.BaseAddress = new Uri(baseUrl);
        });

        // Registrera ditt repository
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICourseServiceClient, CourseServiceClient>();

        return services;
    }
}