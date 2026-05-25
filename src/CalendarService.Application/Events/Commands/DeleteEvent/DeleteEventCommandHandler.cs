using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;

namespace CalendarService.Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler(IEventRepository eventRepository)
{
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<Result<bool>> HandleAsync(DeleteEventCommand command)
    {
        // 1. Be repositoryt ta bort eventet direkt på ID
        var deleteResult = await _eventRepository.DeleteAsync(command.Id);

        if (!deleteResult.Succeeded)
        {
            return Result<bool>.Fail($"Kunde inte ta bort eventet. Det kanske redan är borttaget eller så hittades inte ID: {command.Id}");
        }

        return Result<bool>.Success(true);
    }
}