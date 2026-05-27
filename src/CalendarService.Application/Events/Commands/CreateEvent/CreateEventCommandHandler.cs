using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Application.Interfaces.External;
using CalendarService.Domain.Entities;

namespace CalendarService.Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly ICourseServiceClient _courseServiceClient;

    // 💡 Injicerar både databas-repot och din nya Azure-klient!
    public CreateEventCommandHandler(IEventRepository eventRepository, ICourseServiceClient courseServiceClient)
    {
        _eventRepository = eventRepository;
        _courseServiceClient = courseServiceClient;
    }

    public async Task<Result<bool>> HandleAsync(CreateEventCommand command)
    {
        try
        {
            // 📡 1. Kontrollera och hämta kursens titel direkt från kompisens Azure-moln!
            var actualCourseTitle = await _courseServiceClient.GetCourseTitleAsync(command.CourseId);

            if (actualCourseTitle == null)
            {
                return Result<bool>.Fail($"Kunde inte skapa eventet. Kursen med ID {command.CourseId} existerar inte på Azure.");
            }

            // 🧠 2. Skapa domän-entiteten (Domänreglerna valideras här inuti)
            // Vi använder den riktiga titeln vi precis hämtade från Azure!
            var newEvent = new Event(
                command.CourseId,
                actualCourseTitle,
                command.StartTime,
                command.EndTime
            );

            // 💾 3. Spara i databasen
            var saveResult = await _eventRepository.AddAsync(newEvent);

            if (!saveResult.Succeeded)
            {
                return Result<bool>.Fail("Gick inte att spara eventet i databasen.");
            }

            return Result<bool>.Success(true);
        }
        catch (ArgumentException ex)
        {
            // Fångar upp om t.ex. starttiden var i dåtid eller efter sluttiden
            return Result<bool>.Fail(ex.Message);
        }
    }
}