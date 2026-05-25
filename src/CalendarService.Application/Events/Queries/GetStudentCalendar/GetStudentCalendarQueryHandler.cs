using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Application.Interfaces.External;

namespace CalendarService.Application.Events.Queries.GetStudentCalendar;

public class GetStudentCalendarQueryHandler(
    ICourseServiceClient courseServiceClient,
    IEventRepository eventRepository)
{
    private readonly ICourseServiceClient _courseServiceClient = courseServiceClient;
    private readonly IEventRepository _eventRepository = eventRepository;

    // 💡 Det är denna metod som ditt API kommer att ropa på
    public async Task<Result<IEnumerable<EventDto>>> HandleAsync(GetStudentCalendarQuery query)
    {
        // 1. Fråga externa tjänsten vilka kurser studenten läser
        var coursesResult = await _courseServiceClient.GetUserCoursesIdsAsync(query.UserId);

        if (!coursesResult.Succeeded || coursesResult.Value == null)
        {
            return Result<IEnumerable<EventDto>>.Fail("Kunde inte hämta studentens kurser.");
        }

        var allEvents = new List<EventDto>();

        // 2. Loopa igenom alla kurs-IDn och hämta kalenderevents från VÅR databas
        foreach (var courseId in coursesResult.Value)
        {
            var eventsResult = await _eventRepository.GetByCourseIdAsync(courseId);

            if (eventsResult.Succeeded && eventsResult.Value != null)
            {
                // 3. Mappa om Domän-entiteterna till våra rena EventDto:s
                var dtos = eventsResult.Value.Select(e => new EventDto(
                    e.Id,
                    e.CourseId,
                    e.Title,
                    e.StartTime,
                    e.EndTime
                ));

                allEvents.AddRange(dtos);
            }
        }

        // 4. Returnera den färdiga listan med DTOs, sorterad på starttid!
        return Result<IEnumerable<EventDto>>.Success(allEvents.OrderBy(e => e.StartTime));
    }
}