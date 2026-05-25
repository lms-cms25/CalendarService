using CalendarService.Application.Dtos.Results;
using CalendarService.Domain.Entities;

namespace CalendarService.Application.Interfaces;
public interface IEventRepository
{
    Task<Result<IEnumerable<Event>>> GetByCourseIdAsync(string courseId);
    Task<Result<Event>> GetByIdAsync(string id);
    Task<Result<bool>> AddAsync(Event @event);
    Task<Result<bool>> UpdateAsync(Event @event);
    Task<Result<bool>> DeleteAsync(string id);

}
