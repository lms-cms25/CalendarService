namespace CalendarService.Domain.Entities;

public class LiveClasses(string eventId)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string EventId { get; private set; } = eventId;

    protected LiveClasses() : this(string.Empty) { }
}
