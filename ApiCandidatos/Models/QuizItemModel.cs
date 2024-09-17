#region Documentación
/****************************************************************************************************
* WEBAPI                                                    
****************************************************************************************************
* Unidad        : <.NET/C# modelo para el mapeo de las propiedades de QuizItem>                                                                      
* DescripciÓn   : <Logica de negocio para los servicios de los QuizItem>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <16-09-2024>                                                                             
***************************************************************************************************/
#endregion Documentación
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Web.Api.Models
{
    public class QuizItemModel
    {
        /// <summary>
        /// Identificador único del ítem del quiz.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Pregunta del ítem del quiz.
        /// </summary>
        [Required]
        [StringLength(500, ErrorMessage = "La pregunta no puede tener más de 500 caracteres.")]
        public string Question { get; set; }

        /// <summary>
        /// Índice de la respuesta correcta.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "El índice de respuesta debe ser un número positivo.")]
        public int AnswerIndex { get; set; }

        /// <summary>
        /// Puntuación del ítem del quiz.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "La puntuación debe ser un número positivo.")]
        public int Score { get; set; }

        /// <summary>
        /// Tema de la pregunta.
        /// </summary>
        [StringLength(100, ErrorMessage = "El tema no puede tener más de 100 caracteres.")]
        public string Theme { get; set; }

        /// <summary>
        /// Cadena JSON que representa las opciones de respuesta disponibles.
        /// </summary>>
        public string ChoicesJson { get; set; }

        /// <summary>
        /// Lista de opciones de respuesta. Se convierte a y desde JSON.
        /// </summary>
        [NotMapped]
        public List<string> Choices
        {
            get => string.IsNullOrEmpty(ChoicesJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(ChoicesJson);
            set => ChoicesJson = JsonSerializer.Serialize(value);
        }

    }
}
