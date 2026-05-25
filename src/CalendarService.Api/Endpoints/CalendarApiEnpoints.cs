namespace CalendarService.Api.Endpoints;

public static class CalendarApiEnpoints
{
    public static void MapCalendarEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/calendar")
            .WithTags("Calendar")
            .WithDescription("Templates on how to use Minimal API");
    }
}
