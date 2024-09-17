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
    // Validación del parámetro
    if (string.IsNullOrWhiteSpace(theme))
    {
        return Results.BadRequest("Tema es un parametro requerido.");
    }

    try
    {
        var result = questionService.Get(theme);

        // Verificar si el resultado es nulo o vacío
        if (result == null || !result.Any())
        {
            return Results.NotFound("No existe una pregunta para este tema en especifico.");
        }

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem("Ah ocurrido un error procesando su solicitud.", statusCode: 500);
    }
});

app.MapPost("/api/quiz", async (QuizItemModel request, IServicesQuestion questionService) =>
{
    // Validación del modelo
    if (!IsValidQuizItemModel(request))
    {
        return Results.BadRequest("Invalid request payload.");
    }

    try
    {
        // Llamada al servicio para agregar la pregunta
        var result = await questionService.AddQuestionAsync(request);

        if (result)
        {
            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return Results.Created($"/api/quiz/{request.Question}", request);
        }
        else
        {
            // Devuelve un error 500 si la operación falla
            return Results.Problem("Error al guardar la pregunta en la base de datos.", statusCode: 500);
        }
    }
    catch (Exception ex)
    {
        // Aquí puedes registrar la excepción o tomar otras medidas si es necesario
        return Results.Problem("Ah ocurrido un error procesando su solicitud.", statusCode: 500);
    }
});

bool IsValidQuizItemModel(QuizItemModel model)
{
    return model != null && !string.IsNullOrWhiteSpace(model.Question);
}


app.MapDelete("/api/quiz/{id}", async (Guid id, IServicesQuestion questionService) =>
{
    if (id == Guid.Empty)
    {
        return Results.BadRequest("Este Id no se encuentra registrado.");
    }

    try
    {
        var result = await questionService.DeleteQuestionAsync(id);

        if (result)
        {
            return Results.Ok($"La pregunta con Id {id} ha sido eliminada.");
        }
        else
        {
            return Results.NotFound("La pregunta no existe.");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem("Ah ocurrido un error procesando su solicitud.", statusCode: 500);
    }
});

app.MapControllers(); // Registra los controladores

await app.RunAsync();