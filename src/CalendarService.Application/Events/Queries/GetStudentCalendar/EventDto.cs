namespace CalendarService.Application.Events.Queries.GetStudentCalendar;

public sealed record EventDto(
    string Id,
    string CourseId,
    string Title,
    DateTime StartTime,
    DateTime EndTime
);