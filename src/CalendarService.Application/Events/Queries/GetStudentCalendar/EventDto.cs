namespace CalendarService.Application.Events.Queries.GetStudentCalendar;

public sealed record EventDto(
    string Id,
    int CourseId,
    string Title,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime
);