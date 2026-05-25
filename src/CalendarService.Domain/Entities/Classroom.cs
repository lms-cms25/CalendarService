namespace CalendarService.Domain.Entities;
public class Classroom(string name)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; private set; } = name;
    public ICollection<Events> Events { get; set; } = [];

    protected Classroom() : this(string.Empty) { }
}
