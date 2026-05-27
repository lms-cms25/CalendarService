using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CalendarService.Api.Extensions;

public static class IdentityServiceCollectionExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 💡 1. Sätt upp krypteringsnyckeln (Samma nyckel som din Auth-tjänst använder för att signera tokens!)
        var jwtSecret = configuration["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JWT Secret is missing in configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        // 💡 2. Konfigurera Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                // Om din Auth-tjänst ligger på en specifik URL (t.ex. localhost:5001 eller Auth0) 
                // kan du validera den här. Just nu sätter vi false för att göra det enkelt lokalt:
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateLifetime = true, // 💡 Kontrollera att token inte har gått ut!
                ClockSkew = TimeSpan.Zero // Tar bort standard-fördröjningen på 5 minuter för utgångna tokens
            };
        });

        // 💡 3. Aktivera Authorization
        services.AddAuthorization();

        return services;
    }
}