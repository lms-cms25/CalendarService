namespace CalendarService.Domain.Entities;

public class Event
{
    // EF Core kräver denna tomma
    protected Event() : this(string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue) { }

    // Vår huvudsakliga konstruktor som tvingar fram affärsreglerna vid skapande!
    public Event(string courseId, string title, DateTime startTime, DateTime endTime)
    {
        // 💡 Domänregel 1: Starttid måste vara före sluttid
        if (startTime >= endTime)
        {
            throw new ArgumentException("Ett event kan inte sluta innan eller samtidigt som det startar.");
        }

        // 💡 Domänregel 2: Får inte skapas i dåtid
        if (startTime < DateTime.UtcNow)
        {
            throw new ArgumentException("Du kan inte schemalägga ett event i dåtid.");
        }

        Id = Guid.NewGuid().ToString();
        CourseId = courseId;
        Title = title;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void UpdateDetails(string title, DateTime startTime, DateTime endTime)
    {
        // 💡 Samma domänregler måste gälla vid en uppdatering!
        if (startTime >= endTime)
        {
            throw new ArgumentException("Ett event kan inte sluta innan eller samtidigt som det startar.");
        }

        if (startTime < DateTime.UtcNow)
        {
            throw new ArgumentException("Du kan inte schemalägga ett event i dåtid.");
        }

        Title = title;
        StartTime = startTime;
        EndTime = endTime;
    }

    public string Id { get; private set; }
    public string CourseId { get; private set; }
    public string Title { get; private set; }
    public DateTime StartTime { get; private set; } // Sätt till private set för att skydda datan!
    public DateTime EndTime { get; private set; }   // Sätt till private set för att skydda datan!

    public ICollection<Classroom> Classrooms { get; set; } = [];
}