using CalendarService.Api.Endpoints;
using CalendarService.Api.OpenApi;
using CalendarService.Api.Security;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration();
builder.Services.AddOpenApiConfiguration();

var app = builder.Build();

app.UseCors("All");
app.UseHttpsRedirection();

app.MapOpenApiEndpoints();
app.MapCalendarEndpoints();

app.Run();