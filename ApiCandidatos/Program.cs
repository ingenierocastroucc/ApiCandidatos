using System.Reflection;
using Web.Api.Context;
using Web.Api.Extensions;
using Web.Api.Endpoints.Services;
using Web.Api.Controllers;
using Web.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<WepApiContext>("Data Source=LAPTOP-PH1R9POH;Initial Catalog=VassApi;Integrated Security=True;");

builder.Services.AddScoped<IServicesMiniApi, ServicesMiniApi>();
builder.Services.AddScoped<IServicesQuestion, ServicesQuestion>();

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddSwagger();
builder.Services.AddControllers();

var app = builder.Build();

// Registrar el endpoint usando el contenedor de servicios para obtener el servicio de ámbito
app.MapGet("/api/servicesquestion/{theme}", async (string theme, IServicesQuestion questionService) =>
{
    var result = questionService.Get(theme);
    return Results.Ok(result);
});
// Registrar el endpoint usando el contenedor de servicios para registrar la nueva question
app.MapPost("/api/quiz", async (QuizItemModel request, IServicesQuestion questionService) =>
{
    if (request == null)
    {
        return Results.BadRequest("Invalid request payload");
    }

    var result = await questionService.AddQuestionAsync(request);

    return result ? Results.Created($"/api/quiz/{request.Question}", request) : Results.Problem("Error al guardar la pregunta en la base de datos", statusCode: 500);
});

// Registrar el endpoint usando el contenedor de servicios para eliminar una pregunta
app.MapDelete("/api/quiz/{id}", async (Guid id, IServicesQuestion questionService) =>
{
    if (id == null)
    {
        return Results.BadRequest("Invalid question ID");
    }

    var result = await questionService.DeleteQuestionAsync(id);

    return result ? Results.Ok($"Question with ID {id} successfully deleted.") : Results.NotFound("Question not found");
});

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

await app.RunAsync();
