using CalendarService.Api.Endpoints;
using CalendarService.Api.Extensions;
using CalendarService.Api.OpenApi;
using CalendarService.Api.Security;
using CalendarService.Application.DependencyInjection;
using CalendarService.Infrastructure.Extensions.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration();
builder.Services.AddOpenApiConfiguration();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseCors("All");
//app.UseHttpsRedirection();

app.UseAuthentication(); // "Vem är du?" (Läser JWTn)
app.UseAuthorization();  // "Vad får du göra?" (Kollar rollerna)

app.MapOpenApiEndpoints();
app.MapCalendarEndpoints();

app.Run();