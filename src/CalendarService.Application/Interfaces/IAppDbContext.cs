using CalendarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Application.Interfaces;
public interface IAppDbContext
{
    DbSet<Event> Events { get; set; }
    DbSet<StudentCourse> StudentCourses { get; set; } // Vår nya tabell!

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
