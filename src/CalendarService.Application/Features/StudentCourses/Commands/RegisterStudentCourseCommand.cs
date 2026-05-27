using CalendarService.Application.Interfaces;
using CalendarService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Application.Features.StudentCourses.Commands;

// Detta är själva "beställningen" som skickas in från API-lagret
public record RegisterStudentCourseCommand(string UserId, int CourseId) : IRequest<bool>;

// Detta är "arbetaren" som utför jobbet
public class RegisterStudentCourseCommandHandler : IRequestHandler<RegisterStudentCourseCommand, bool>
{
    private readonly IAppDbContext _context; // Byt ut mot ditt faktiska DbContext-namn om du inte kör interface

    public RegisterStudentCourseCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RegisterStudentCourseCommand request, CancellationToken cancellationToken)
    {
        // 1. Idempotens-kontroll: Kolla om registreringen redan finns så vi inte dubbelregistrerar
        var exists = await _context.StudentCourses
            .AnyAsync(sc => sc.UserId == request.UserId && sc.CourseId == request.CourseId, cancellationToken);

        if (exists)
        {
            return true; // Redan registrerad, vi är nöjda!
        }

        // 2. Skapa den nya domänentiteten (här kickar dina valideringar in!)
        var studentCourse = new StudentCourse(request.UserId, request.CourseId);

        // 3. Spara i databasen
        _context.StudentCourses.Add(studentCourse);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}