using CalendarService.Application.Dtos.Results;

namespace CalendarService.Application.Interfaces.External;

public interface ICourseServiceClient
{
    Task<string?> GetCourseTitleAsync(int courseId);
    Task<Result<IEnumerable<int>>> GetUserCoursesIdsAsync(string userId);
}