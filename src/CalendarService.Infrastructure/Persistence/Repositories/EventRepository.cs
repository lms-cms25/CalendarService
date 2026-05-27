using CalendarService.Application.Dtos.Results;
using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;
using CalendarService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Infrastructure.Persistence.Repositories;

public class EventRepository(AppDbContext context) : IEventRepository
{

    private readonly AppDbContext _context = context;

    // 💡 1. Hämta alla events för en specifik kurs
    public async Task<Result<IEnumerable<Event>>> GetByCourseIdAsync(int courseId)
    {
        try
        {
            var events = await _context.Events
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

            return Result<IEnumerable<Event>>.Success(events);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Event>>.Fail($"Kunde inte hämta events från databasen: {ex.Message}");
        }
    }

    // 💡 2. Hämta ett specifikt event baserat på dess ID
    public async Task<Result<Event>> GetByIdAsync(string id)
    {
        try
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return Result<Event>.Fail($"Hittade inget event med ID {id}");
            }

            return Result<Event>.Success(@event);
        }
        catch (Exception ex)
        {
            return Result<Event>.Fail($"Fel vid hämtning av event: {ex.Message}");
        }
    }

    // 💡 3. Lägg till ett nytt event (Används av CreateEvent)
    public async Task<Result<bool>> AddAsync(Event @event)
    {
        try
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync(); // 💡 Sparar ändringarna i minnet/databasen

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Kunde inte spara eventet: {ex.Message}");
        }
    }

    // 💡 4. Uppdatera ett befintligt event (Används av UpdateEvent)
    public async Task<Result<bool>> UpdateAsync(Event @event)
    {
        try
        {
            // EF Core spårar oftast objektet automatiskt, men Update() säkerställer att det sparas
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Kunde inte uppdatera eventet: {ex.Message}");
        }
    }

    // 💡 5. Ta bort ett event (Används av DeleteEvent)
    public async Task<Result<bool>> DeleteAsync(string id)
    {
        try
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return Result<bool>.Fail("Eventet hittades inte och kunde inte tas bort.");
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Kunde inte ta bort eventet: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }
}
