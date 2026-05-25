using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Application.Events.Queries.GetStudentCalendar; // 💡 Importera vår EventDto

namespace CalendarService.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler(IEventRepository eventRepository)
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<Result<EventDto>> HandleAsync(GetEventByIdQuery query)
    {
        var repositoryResult = await _eventRepository.GetByIdAsync(query.Id);

        // 1. Om eventet inte hittades eller något gick fel i databasen
        if (!repositoryResult.Succeeded || repositoryResult.Value == null)
        {
            return Result<EventDto>.Fail($"Kalenderhändelsen med ID {query.Id} hittades inte.");
        }

        var @event = repositoryResult.Value;

        // 2. Mappa om domänmodellen till vår DTO
        var dto = new EventDto(
            @event.Id,
            @event.CourseId,
            @event.Title,
            @event.StartTime,
            @event.EndTime
        );

        return Result<EventDto>.Success(dto);
    }
}