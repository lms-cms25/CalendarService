using System.Net.Http.Json;
using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces.External;

namespace CalendarService.Infrastructure.Services.External;

public class CourseServiceClient : ICourseServiceClient
{
    private readonly HttpClient _httpClient;

    public CourseServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetCourseTitleAsync(int courseId)
    {
        //try
        //{
        //    var response = await _httpClient.GetAsync($"api/courses/{courseId}");

        //    if (!response.IsSuccessStatusCode)
        //        return null;

        //    var course = await response.Content.ReadFromJsonAsync<CourseData>();
        //    return course?.Title;
        //}
        //catch (HttpRequestException)
        //{
        //    return null;
        //}
        return courseId switch
        {
            1 => "Machine Learning Basics",
            2 => "Business Analytics & Strategy",
            3 => "Content Marketing",
            _ => $"Kurs {courseId} (Generisk titel)"
        };
    }

    public async Task<Result<IEnumerable<int>>> GetUserCoursesIdsAsync(string userId)
    {
        try
        {
            // 📡 Vi gör ett anrop till kompisens API för att hämta den aktuella studentens kurser
            // OBS: Dubbelkolla med henne om hennes rutt är exakt så här, eller om det är t.ex. api/courses?userId=...
            var response = await _httpClient.GetAsync($"api/courses/student/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return Result<IEnumerable<int>>.Fail($"Kunde inte hämta kurser för användare {userId} från Azure.");
            }

            // Vi läser ut listan med kurs-IDn från svaret.
            // Vi antar att hennes API returnerar en lista med objekt där 'id' eller en ren array med ints ingår.
            // Om hon returnerar en lista med hela kurs-objekt, mappar vi ut bara ID:na här:
            var studentCourses = await response.Content.ReadFromJsonAsync<IEnumerable<StudentCourseDto>>();

            if (studentCourses == null)
            {
                return Result<IEnumerable<int>>.Success(Enumerable.Empty<int>());
            }

            var courseIds = studentCourses.Select(c => c.Id);

            return Result<IEnumerable<int>>.Success(courseIds);
        }
        catch (HttpRequestException ex)
        {
            return Result<IEnumerable<int>>.Fail($"Nätverksfel vid hämtning av studentens kurser: {ex.Message}");
        }
    }

    // 💡 En till liten privat hjälpare i botten av filen (eller inuti klassen) 
    // för att kunna läsa ut ID från kompisens kurs-svar
    private record StudentCourseDto(int Id);
    private record CourseData(string Title);
}