using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CalendarService.Infrastructure.Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{

    // 💡 Här talar vi om vilka tabeller som ska finnas i databasen
    public DbSet<Event> Events { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<LiveClass> LiveClasses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    // Denna metod används för att konfigurera detaljer i tabellerna (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
