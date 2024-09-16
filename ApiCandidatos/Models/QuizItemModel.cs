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
        /// Propiedad para el id del quiz Item
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Propiedad para la pregunta del ususario
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Propiedad para obtener el horario de disponibilidad
        /// </summary>
        public int AnswerIndex { get; set; }

        /// <summary>
        /// Propiedad para obtener el horario de disponibilidad
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Propiedad para el tema de la pregunta
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Propiedad para las opciones de respuesta disponibles.
        /// </summary>
        public string ChoicesJson { get; set; }

        /// <summary>
        /// propiedad para la lista de cadenas. Internamente, esta lista se convierte a JSON.
        /// </summary>
        [NotMapped]
        public List<string> Choices
        {
            get => string.IsNullOrEmpty(ChoicesJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(ChoicesJson);
            set => ChoicesJson = JsonSerializer.Serialize(value);
        }

    }
}
