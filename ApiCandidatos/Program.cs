using System.Reflection;
using Web.Api.Context;
using Web.Api.Extensions;
using Web.Api.Endpoints.Services;
using Web.Api.Controllers;
using Web.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura los servicios
builder.Services.AddCors(builder.Configuration); // Llama al método de extensión
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<WepApiContext>("Data Source=LAPTOP-PH1R9POH;Initial Catalog=ApiCandidatos;Integrated Security=True;TrustServerCertificate=True;");
builder.Services.AddScoped<IServicesMiniApi, ServicesMiniApi>();
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

app.MapGet("/api/testcors", (HttpContext context) =>
{
    return Results.Ok("CORS está funcionando correctamente.");
});

app.MapControllers(); // Registra los controladores

await app.RunAsync();