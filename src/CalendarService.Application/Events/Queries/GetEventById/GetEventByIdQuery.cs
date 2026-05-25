namespace CalendarService.Application.Events.Queries.GetEventById;

// 💡 Vi behöver bara ID:t på det specifika eventet vi letar efter
public sealed record GetEventByIdQuery(string Id);