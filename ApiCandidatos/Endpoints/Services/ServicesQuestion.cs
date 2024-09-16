#region Documentación
/****************************************************************************************************
* WEBAPI                                                    
****************************************************************************************************
* Unidad        : <.NET/C# para los servicios de las QuizItem>                                                                      
* DescripciÓn   : <Logica de negocio para los servicios de los QuizItem>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <03-09-2024>
* ===========   ============       ========================================================================= 
 * 04/09/2024   Pedro Castro       1. Se agrega service posts
***************************************************************************************************/
#endregion Documentación

using Web.Api.Context;
using Web.Api.Models;

namespace Web.Api.Endpoints.Services
{
    public class ServicesQuestion : IServicesQuestion
    {
        WepApiContext context;


        ///Acceso a la base de datos
        public ServicesQuestion(WepApiContext dbcontext)
        {
            context = dbcontext;
        }

        ///Obtencion del listado de QuizItem por tema
        public IEnumerable<QuizItemModel> Get(string theme)
        {
            var themeLower = theme.ToLower();
            return context.QuizItemModelVirtual
                .Where(item => item.Theme.ToLower() == themeLower);
        }

        public async Task<bool> AddQuestionAsync(QuizItemModel question)
        {
            // Implementa aquí la lógica para agregar la pregunta.
            context.QuizItemModelVirtual.Add(question);
            await context.SaveChangesAsync();
            return true;
        }

        // Eliminación de una pregunta
        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await context.QuizItemModelVirtual.FindAsync(id);

            if (question == null)
            {
                return false; // La pregunta no fue encontrada
            }

            context.QuizItemModelVirtual.Remove(question);
            await context.SaveChangesAsync();
            return true; // La pregunta fue eliminada
        }

    }
    //Llamado de los servicios
    public interface IServicesQuestion
    {
        IEnumerable<QuizItemModel> Get(string theme);
        Task <bool> AddQuestionAsync(QuizItemModel question);

        Task<bool> DeleteQuestionAsync(Guid id);
    }
}
