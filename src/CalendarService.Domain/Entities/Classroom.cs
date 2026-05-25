namespace CalendarService.Domain.Entities;
public class Classroom(string name)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; private set; } = name;
    public ICollection<Event> Events { get; set; } = [];

    protected Classroom() : this(string.Empty) { }
}
