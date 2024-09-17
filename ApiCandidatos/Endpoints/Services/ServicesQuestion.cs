#region Documentación
/****************************************************************************************************
* WEBAPI                                                    
****************************************************************************************************
* Unidad        : .NET/C# para los servicios de los QuizItem                                                                      
* Descripción   : Lógica de negocio para los servicios de los QuizItem                                                      
* Autor         : Pedro Castro
* Fecha         : 16-09-2024
* ===========   ============       ========================================================================= 
* 17/09/2024   Pedro Castro       1. Se agrega servicio para la gestión de preguntas
***************************************************************************************************/
#endregion Documentación

using Web.Api.Context;
using Web.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Endpoints.Services
{
    public class ServicesQuestion : IServicesQuestion
    {
        WebApiContext context;


        /// <summary>
        /// Constructor que recibe el contexto de la base de datos.
        /// </summary>
        /// <param name="dbcontext">Contexto de la base de datos.</param>
        public ServicesQuestion(WebApiContext dbcontext)
        {
            context = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        /// <summary>
        /// Obtiene una lista de preguntas (QuizItems) filtradas por tema.
        /// </summary>
        /// <param name="theme">Tema para filtrar las preguntas.</param>
        /// <returns>Lista de preguntas que coinciden con el tema especificado.</returns>
        public IEnumerable<QuizItemModel> Get(string theme)
        {
            if (string.IsNullOrEmpty(theme))
            {
                throw new ArgumentException("El tema no puede ser nulo o vacío.", nameof(theme));
            }

            var themeLower = theme.ToLower();
            return context.QuizItems
                .Where(item => item.Theme.ToLower() == themeLower);
        }

        /// <summary>
        /// Agrega una nueva pregunta al sistema de manera asíncrona.
        /// </summary>
        /// <param name="question">Objeto de la pregunta a agregar.</param>
        /// <returns>Resultado de la operación de adición.</returns>
        public async Task<bool> AddQuestionAsync(QuizItemModel question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            context.QuizItems.Add(question);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Elimina una pregunta existente de manera asíncrona.
        /// </summary>
        /// <param name="id">ID de la pregunta a eliminar.</param>
        /// <returns>Resultado de la operación de eliminación.</returns>
        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await context.QuizItems.FindAsync(id);

            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            context.QuizItems.Remove(question);
            await context.SaveChangesAsync();
            return true; // La pregunta fue eliminada
        }

    }

    /// <summary>
    /// Interfaz para los servicios relacionados con las preguntas (QuizItems).
    /// </summary>
    public interface IServicesQuestion
    {
        IEnumerable<QuizItemModel> Get(string theme);
        Task <bool> AddQuestionAsync(QuizItemModel question);

        Task<bool> DeleteQuestionAsync(Guid id);
    }
}
