using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;

namespace CalendarService.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler(IEventRepository eventRepository)
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<Result<bool>> HandleAsync(UpdateEventCommand command)
    {
        // 1. Hämta det befintliga eventet från databasen först
        var existingEventResult = await _eventRepository.GetByIdAsync(command.Id);

        if (!existingEventResult.Succeeded || existingEventResult.Value == null)
        {
            return Result<bool>.Fail("Eventet hittades inte och kunde inte uppdateras.");
        }

        var @event = existingEventResult.Value;

        try
        {
            // 2. Be domänmodellen uppdatera sig själv (här körs valideringen!)
            @event.UpdateDetails(command.Title, command.StartTime, command.EndTime);

            // 3. Spara ändringarna via repositoryt
            var updateResult = await _eventRepository.UpdateAsync(@event);

            if (!updateResult.Succeeded)
            {
                return Result<bool>.Fail("Kunde inte spara uppdateringarna i databasen.");
            }

            return Result<bool>.Success(true);
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Fail(ex.Message);
        }
    }
}