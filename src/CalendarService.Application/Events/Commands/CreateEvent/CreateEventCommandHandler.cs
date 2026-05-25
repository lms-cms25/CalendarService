using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;

namespace CalendarService.Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler(IEventRepository eventRepository)
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<Result<bool>> HandleAsync(CreateEventCommand command)
    {
        try
        {
            // 💡 Vi försöker skapa domän-entiteten. Om reglerna bryts kommer den att kasta ett exception här!
            var newEvent = new Event(
                command.CourseId,
                command.Title,
                command.StartTime,
                command.EndTime
            );

            // Spara i databasen
            var saveResult = await _eventRepository.AddAsync(newEvent);

            if (!saveResult.Succeeded)
            {
                return Result<bool>.Fail("Gick inte att spara eventet i databasen.");
            }

            return Result<bool>.Success(true);
        }
        catch (ArgumentException ex)
        {
            // 💡 Här fångar vi domänens protest och returnerar det som ett snyggt felmeddelande!
            return Result<bool>.Fail(ex.Message);
        }
    }
}