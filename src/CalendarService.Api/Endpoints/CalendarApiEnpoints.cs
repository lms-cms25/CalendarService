using CalendarService.Application.Dtos.Requests;
using CalendarService.Application.Events.Commands.CreateEvent;
using CalendarService.Application.Events.Commands.DeleteEvent;
using CalendarService.Application.Events.Commands.UpdateEvent;
using CalendarService.Application.Events.Queries.GetAllEvents;
using CalendarService.Application.Events.Queries.GetEventById;
using CalendarService.Application.Events.Queries.GetStudentCalendar;
using CalendarService.Application.Features.StudentCourses.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization; // 💡 Behövs för AuthorizeAttribute
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalendarService.Api.Endpoints;

public static class CalendarApiEnpoints
{
    public static void MapCalendarEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/calendar")
            .WithTags("Calendar")
            .WithDescription("Endpoints för att hantera studenters kalendrar och events");

        // 💡 1. GET: Hämta en students hela kalender
        // Tillåter: Student, Instructor, Admin (Alla inloggade roller)
        group.MapGet("/student/{userId}", async (
            string userId,
            [FromServices] GetStudentCalendarQueryHandler handler) =>
        {
            var query = new GetStudentCalendarQuery(userId);
            var result = await handler.HandleAsync(query);

            return result.Succeeded
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.ErrorMessage);
        });
        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Student,Instructor,Admin" });

        // 💡 2. GET: Hämta ett specifikt event på ID
        // Tillåter: Student, Instructor, Admin
        group.MapGet("/event/{id}", async (
            string id,
            [FromServices] GetEventByIdQueryHandler handler) =>
        {
            var query = new GetEventByIdQuery(id);
            var result = await handler.HandleAsync(query);

            return result.Succeeded
                ? Results.Ok(result.Value)
                : Results.NotFound(result.ErrorMessage);
        });
        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Student,Instructor,Admin" });

        // 💡 3. POST: Skapa ett nytt event
        // Tillåter: ENDAST Instructor och Admin
        group.MapPost("/event", async (
            [FromBody] CreateEventCommand command,
            [FromServices] CreateEventCommandHandler handler) =>
        {
            var result = await handler.HandleAsync(command);

            return result.Succeeded
                ? Results.Created($"/api/calendar/event", result)
                : Results.BadRequest(result.ErrorMessage);
        });
        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Instructor,Admin" });

        // 💡 4. PUT: Uppdatera ett befintligt event
        // Tillåter: ENDAST Instructor och Admin
        group.MapPut("/event", async (
            [FromBody] UpdateEventCommand command,
            [FromServices] UpdateEventCommandHandler handler) =>
        {
            var result = await handler.HandleAsync(command);

            return result.Succeeded
                ? Results.Ok("Eventet har uppdaterats framgångsrikt.")
                :
                Results.BadRequest(result.ErrorMessage);
        });
        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Instructor,Admin" });

        // 💡 5. DELETE: Ta bort ett event
        // Tillåter: ENDAST Instructor och Admin
        group.MapDelete("/event/{id}", async (
            string id,
            [FromServices] DeleteEventCommandHandler handler) =>
        {
            var command = new DeleteEventCommand(id);
            var result = await handler.HandleAsync(command);

            return result.Succeeded
                ? Results.Ok("Eventet har tagits bort.")
                : Results.BadRequest(result.ErrorMessage);
        });
        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Instructor,Admin" });

        // 💡 NY: GET: Hämta ALLA events i hela databasen (Öppet schema)
        group.MapGet("/all", async (
            [FromServices] GetAllEventsQueryHandler handler) =>
        {
            var result = await handler.HandleAsync();

            return result.Succeeded
                ? Results.Ok(result.Value)        // Ändrade till result.Value för att matcha din Result-struktur
                : Results.BadRequest(result.ErrorMessage); // Ändrade till result.ErrorMessage
        });


        // 1. POST: Registrera en student på en kurs internt
        group.MapPost("/student/register", async (
            RegisterStudentCourseRequest model,
            [FromServices] IMediator mediator) =>
        {
            var command = new RegisterStudentCourseCommand(model.UserId, model.CourseId);
            var result = await mediator.Send(command);

            return result
                ? Results.Ok(new { Message = $"Användare {model.UserId} har registrerats på kurs {model.CourseId} internt i kalendern." })
                : Results.BadRequest("Kunde inte slutföra registreringen.");
        })
        .WithName("RegisterStudentCourse");

        // 2. GET: Hämta kalendern baserat på UserId
        group.MapGet("/student/{userId}/calendar", async (
            string userId,
            [FromServices] IMediator mediator) =>
        {
            var query = new GetStudentCalendarQuery(userId);
            var calendar = await mediator.Send(query);

            return Results.Ok(calendar);
        })
        .WithName("GetStudentCalendar"); 
    }

}