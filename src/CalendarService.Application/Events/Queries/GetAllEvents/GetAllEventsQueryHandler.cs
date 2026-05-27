using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;

namespace CalendarService.Application.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler
{
    private readonly IEventRepository _eventRepository;

    public GetAllEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<IEnumerable<object>>> HandleAsync()
    {
        try
        {
            // Vi ber ditt repository att ge oss en Queryable, eller så mappar vi om det.
            // Om ditt repository har en metod som heter GetQueryable() eller liknande kan du köra på den.
            // Om inte, kan vi köra din gamla _eventRepository.GetAllAsync() fast vi mappar om den i nästa steg.

            // LÅT OSS GÖRA DET HÄR ISTÄLLET: 
            // Eftersom ditt repository returnerar fel vid GetAllAsync, låt oss hämta datan som ett anonymt objekt 
            // direkt via handlern om du har tillgång till DbContext, ELLER så gör vi så här:

            var events = await _eventRepository.GetAllAsync();

            var cleanEvents = events.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                startTime = e.StartTime,
                endTime = e.EndTime,
                courseId = e.CourseId
            });

            return Result<IEnumerable<object>>.Success(cleanEvents);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<object>>.Fail($"Kunde inte hämta schemat: {ex.Message}");
        }
    }
}