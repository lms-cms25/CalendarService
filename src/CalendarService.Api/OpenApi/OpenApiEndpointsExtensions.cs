using Scalar.AspNetCore;

namespace CalendarService.Api.OpenApi;

public static class OpenApiEndpointsExtensions
{
    public static WebApplication MapOpenApiEndpoints(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference("/docs", options => options
            .WithTitle("Documentation for API"));
        
        return app;
    }
}
