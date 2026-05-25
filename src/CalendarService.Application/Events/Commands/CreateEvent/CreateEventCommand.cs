namespace CalendarService.Application.Events.Commands.CreateEvent;

// 💡 Innehåller all data som krävs för att kunna skapa ett nytt event i databasen
public sealed record CreateEventCommand(
    string CourseId,
    string Title,
    DateTime StartTime,
    DateTime EndTime
);