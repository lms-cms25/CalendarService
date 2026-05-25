namespace CalendarService.Domain.Entities;

public class Events(string courseId, string title, DateTime startTime, DateTime endTime)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string CourseId { get; private set; } = courseId;
    public ICollection<Classroom> Classrooms { get; set; } = [];
    public string Title { get; private set; } = title;

    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;

    protected Events() : this(string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue) { }
}
