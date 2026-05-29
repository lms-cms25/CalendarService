namespace CalendarService.Domain.Entities;

public class StudentCourse
{
    // EF Core kräver en tom eller protected konstruktor vid materialisering från DB
    protected StudentCourse()
    {
        Id = null!;
        UserId = null!;
    }

    // Vår huvudsakliga konstruktor som tvingar fram domänvalidering
    public StudentCourse(string studentId, int courseId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException("StudentId (Guid) måste bifogas.");
        }

        if (courseId <= 0)
        {
            throw new ArgumentException("Ogiltigt CourseId. Måste vara ett positivt heltal.");
        }

        Id = Guid.NewGuid().ToString();
        UserId = studentId;
        CourseId = courseId;
    }

    public string Id { get; private set; }
    public string UserId { get; private set; } // Sparar den externa användarens Guid som sträng
    public int CourseId { get; private set; }
}