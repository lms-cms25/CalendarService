using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Application.Events.Queries.GetStudentCalendar;

// 1. Vi talar om för MediatR att detta är en godkänd arbetare för denna query
public class GetStudentCalendarQueryHandler : IRequestHandler<GetStudentCalendarQuery, Result<IEnumerable<EventDto>>>
{
    private readonly IAppDbContext _context;

    // 2. Vi injectar vårt nya IAppDbContext för att läsa direkt från vår SQLite-databas
    public GetStudentCalendarQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    // 3. MediatR kräver att metoden heter Handle och matchar interfacet
    public async Task<Result<IEnumerable<EventDto>>> Handle(GetStudentCalendarQuery request, CancellationToken cancellationToken)
    {
        // Steg 1: Hämta alla kurs-IDn som den här studenten är registrerad på från VÅR lokala tabell
        var studentCourseIds = await _context.StudentCourses
            .Where(sc => sc.UserId == request.UserId)
            .Select(sc => sc.CourseId)
            .ToListAsync(cancellationToken);

        if (!studentCourseIds.Any())
        {
            // Om studenten inte är registrerad på några kurser, returnera en tom lista (inget schema)
            return Result<IEnumerable<EventDto>>.Success(Enumerable.Empty<EventDto>());
        }

        // Steg 2: Hämta alla events från databasen som matchar studentens kurser i en enda snabb databasfråga!
        // (SQL-motsvarigheten till en "IN"-clausa: WHERE CourseId IN (1, 2, 3))
        var calendarEvents = await _context.Events
            .Where(e => studentCourseIds.Contains(e.CourseId))
            .OrderBy(e => e.StartTime)
            .ToListAsync(cancellationToken);

        // Steg 3: Mappa om domän-entiteterna till rena, fina EventDto:s
        var eventDtos = calendarEvents.Select(e => new EventDto(
            e.Id,
            e.CourseId,
            e.Title,
            e.StartTime,
            e.EndTime
        ));

        // Steg 4: Returnera succé-resultatet!
        return Result<IEnumerable<EventDto>>.Success(eventDtos);
    }
}