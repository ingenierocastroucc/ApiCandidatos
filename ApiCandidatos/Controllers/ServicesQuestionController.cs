#region Documentación
/****************************************************************************************************
* WEBAPI                                                    
****************************************************************************************************
* Unidad        : <.NET/C# para los servicios de las QuizItem>                                                                      
* DescripciÓn   : <Logica de negocio para los servicios de los QuizItem>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <03-09-2024>
* ===========   ============       ========================================================================= 
 * 04/09/2024   Pedro Castro       1. Se agrega service MapPost
***************************************************************************************************/
#endregion Documentación
using Web.Api.Endpoints.Services;
using Web.Api.Endpoints;
using Web.Api.Models;

namespace Web.Api.Controllers
{
    public class ServicesQuestionController : IEndpoint
    {
        private readonly IServicesQuestion questionService;

        public ServicesQuestionController(IServicesQuestion questionService)
        {
            questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/servicesquestion/{theme}", async (string theme, HttpContext context) =>
            {
                if (string.IsNullOrWhiteSpace(theme))
                {
                    // Retorna un error 400 Bad Request si 'theme' es nulo o vacío
                    return Results.BadRequest("El parámetro 'theme' no puede estar vacío.");
                }

                var result = questionService.Get(theme);
                return Results.Ok(result);
            });

            app.MapPost("/api/quiz", async (QuizItemModel request, HttpContext context) =>
            {
                if (request == null)
                {
                    return Results.BadRequest("Invalid request payload");
                }

                // Aquí se debería llamar al servicio para manejar la lógica de agregar la pregunta
                var result = await questionService.AddQuestionAsync(request);

                return result ? Results.Created($"/api/quiz/{request.Question}", request) : Results.StatusCode(500);
            });

            app.MapDelete("/api/quiz/{id}", async (Guid id, HttpContext context) =>
            {
                if (id == Guid.Empty)
                {
                    return Results.BadRequest("Invalid question ID");
                }

                var result = await questionService.DeleteQuestionAsync(id);

                return result ? Results.NoContent() : Results.NotFound("Question not found");
            });

        }

    }
}
