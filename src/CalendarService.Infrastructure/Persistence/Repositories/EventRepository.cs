using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;

namespace CalendarService.Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    public Task<Result<bool>> AddAsync(Event @event)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Event>>> GetByCourseIdAsync(string courseId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Event>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateAsync(Event @event)
    {
        throw new NotImplementedException();
    }
}
