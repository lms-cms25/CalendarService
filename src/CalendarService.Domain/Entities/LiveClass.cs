namespace CalendarService.Domain.Entities;

public class LiveClass(string eventId)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string EventId { get; private set; } = eventId;

    protected LiveClass() : this(string.Empty) { }
}
