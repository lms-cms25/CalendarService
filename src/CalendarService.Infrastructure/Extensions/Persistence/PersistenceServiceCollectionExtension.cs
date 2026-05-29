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
                // 💡 Vid lokal utveckling kör vi SQLite! Supersmidigt, sparar i en lokal fil 'calendar.db'
                options.UseSqlite("Data Source=calendar.db");
            }
            else
            {
                // 💡 I Azure (Production) använder vi en riktig SQL Server via din anslutningssträng
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            }
        });

        // 💡 Registrera gränssnittet IAppDbContext så att dina Handlers i Application kan prata med databasen
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        // Registrera din HTTP-klient mot CourseService (Azure-länken från din secrets.json)
        services.AddHttpClient<ICourseServiceClient, CourseServiceClient>(client =>
        {
            var baseUrl = configuration["ExternalServices:CourseServiceUrl"]
                ?? throw new InvalidOperationException("Konfigurationen 'ExternalServices:CourseServiceUrl' saknas i dina inställningar.");

            client.BaseAddress = new Uri(baseUrl);
        });

        // Registrera dina repositories och externa tjänster
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICourseServiceClient, CourseServiceClient>();

        return services;
    }
}