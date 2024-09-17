using System.Reflection;
using Web.Api.Context;
using Web.Api.Extensions;
using Web.Api.Endpoints.Services;
using Web.Api.Controllers;
using Web.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexión y el contexto de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WebApiContext>(options =>
    options.UseSqlServer(connectionString));

// Configura los servicios
builder.Services.AddCors(builder.Configuration); // Llama al método de extensión
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IServicesQuestion, ServicesQuestion>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();


// Configura el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// Aplica la política de CORS
app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapGet("/api/servicesquestion/{theme}", async (string theme, IServicesQuestion questionService) =>
{
    var result = questionService.Get(theme);
    return Results.Ok(result);
});

app.MapPost("/api/quiz", async (QuizItemModel request, IServicesQuestion questionService) =>
{
    if (request == null)
    {
        return Results.BadRequest("Invalid request payload");
    }

    var result = await questionService.AddQuestionAsync(request);
    return result ? Results.Created($"/api/quiz/{request.Question}", request) : Results.Problem("Error al guardar la pregunta en la base de datos", statusCode: 500);
});

app.MapDelete("/api/quiz/{id}", async (Guid id, IServicesQuestion questionService) =>
{
    if (id == Guid.Empty)
    {
        return Results.BadRequest("Invalid question ID");
    }

    var result = await questionService.DeleteQuestionAsync(id);
    return result ? Results.Ok($"Question with ID {id} successfully deleted.") : Results.NotFound("Question not found");
});

app.MapControllers(); // Registra los controladores

await app.RunAsync();