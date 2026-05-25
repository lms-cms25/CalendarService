namespace CalendarService.Application.Events.Commands.UpdateEvent;

public sealed record UpdateEventCommand(
    string Id,
    string Title,
    DateTime StartTime,
    DateTime EndTime
);