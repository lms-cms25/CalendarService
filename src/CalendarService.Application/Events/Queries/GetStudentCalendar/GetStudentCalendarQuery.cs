using CalendarService.Application.Dtos.Results;
using MediatR;

namespace CalendarService.Application.Events.Queries.GetStudentCalendar;

public sealed record GetStudentCalendarQuery(string UserId) : IRequest<Result<IEnumerable<EventDto>>>;