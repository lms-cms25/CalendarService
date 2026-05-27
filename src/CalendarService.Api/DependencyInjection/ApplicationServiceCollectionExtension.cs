using CalendarService.Application.Events.Commands.CreateEvent;
using CalendarService.Application.Events.Commands.DeleteEvent;
using CalendarService.Application.Events.Commands.UpdateEvent;
using CalendarService.Application.Events.Queries.GetAllEvents;
using CalendarService.Application.Events.Queries.GetEventById;
using CalendarService.Application.Events.Queries.GetStudentCalendar;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarService.Api.DependencyInjection;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 💡 Här registrerar vi alla Handlers lokalt i Application-lagret!
        services.AddScoped<CreateEventCommandHandler>();
        services.AddScoped<UpdateEventCommandHandler>();
        services.AddScoped<DeleteEventCommandHandler>();
        services.AddScoped<GetStudentCalendarQueryHandler>();
        services.AddScoped<GetEventByIdQueryHandler>();
        services.AddScoped<GetAllEventsQueryHandler>();
        return services;
    }
}