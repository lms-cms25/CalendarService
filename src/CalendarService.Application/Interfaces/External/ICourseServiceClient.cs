using CalendarService.Application.Dtos.Results;

namespace CalendarService.Application.Interfaces.External;
public interface ICourseServiceClient
{
    Task<Result<IEnumerable<string>>> GetUserCoursesIdsAsync(string userId);
}
