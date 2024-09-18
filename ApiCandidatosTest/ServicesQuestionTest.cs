#region Documentación
/****************************************************************************************************
* WEBAPI                                                    
****************************************************************************************************
* Unidad        : <.NET/C# para los servicios de las QuizItem>                                                                      
* DescripciÓn   : <Logica de negocio para los test de las QuizItem>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <18-09-2024>
* ===========   ============       ========================================================================= 
***************************************************************************************************/
#endregion Documentación
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// Interfaz para el servicio de preguntas.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Obtiene una pregunta basada en el tema proporcionado.
    /// </summary>
    /// <param name="theme">El tema de la pregunta.</param>
    /// <returns>Un objeto que representa la pregunta.</returns>
    object Get(string theme);

    /// <summary>
    /// Elimina una pregunta dada su ID.
    /// </summary>
    /// <param name="id">El ID de la pregunta.</param>
    /// <returns>Un valor booleano que indica si la eliminación fue exitosa.</returns>
    Task<bool> DeleteAsync(string id);
}

/// <summary>
/// Clase de pruebas para el servicio de preguntas.
/// </summary>
public class ServiciosPreguntaTests
{
    private readonly Mock<IQuestionService> _mockQuestionService;

    public ServiciosPreguntaTests()
    {
        // Inicializamos el mock del servicio de preguntas.
        _mockQuestionService = new Mock<IQuestionService>();
    }

    /// <summary>
    /// Prueba que verifica que el endpoint de obtener pregunta retorna un resultado OK con los datos esperados.
    /// </summary>
    [Fact]
    public async Task ObtenerPregunta()
    {
        // Arrange
        var tema = "testTema";
        dynamic resultadoEsperado = CrearResultadoEsperado();

        // Configuramos el comportamiento del mock para el método Get.
        _mockQuestionService.Setup(servicio => servicio.Get(tema)).Returns(resultadoEsperado);

        // Act
        var resultado = await EjecutarEndpointObtenerPregunta(tema);

        // Assert
        AssertOkResultConDatosEsperados(resultado, resultadoEsperado);
    }

    /// <summary>
    /// Prueba que verifica que el endpoint de eliminar pregunta retorna BadRequest para un ID inválido.
    /// </summary>
    [Fact]
    public async Task EliminarPregunta()
    {
        // Arrange
        var idInvalido = Guid.Empty.ToString();

        // Act
        var resultado = await EjecutarEndpointEliminarPregunta(idInvalido);

        // Assert
        AssertBadRequestParaIdInvalido(resultado);
    }

    /// <summary>
    /// Crea un resultado esperado para las pruebas.
    /// </summary>
    /// <returns>Un objeto dinámico que representa la pregunta esperada.</returns>
    private dynamic CrearResultadoEsperado()
    {
        dynamic resultadoEsperado = new ExpandoObject();
        resultadoEsperado.Pregunta = "¿Cuál es el tema?";
        return resultadoEsperado;
    }

    /// <summary>
    /// Simula la lógica del endpoint para obtener una pregunta.
    /// </summary>
    /// <param name="tema">El tema de la pregunta.</param>
    /// <returns>Un resultado de acción que representa la respuesta del endpoint.</returns>
    private async Task<IActionResult> EjecutarEndpointObtenerPregunta(string tema)
    {
        var resultado = _mockQuestionService.Object.Get(tema);
        return new OkObjectResult(resultado);
    }

    /// <summary>
    /// Simula la lógica del endpoint para eliminar una pregunta.
    /// </summary>
    /// <param name="id">El ID de la pregunta.</param>
    /// <returns>Un resultado de acción que representa la respuesta del endpoint.</returns>
    private async Task<IActionResult> EjecutarEndpointEliminarPregunta(string id)
    {
        if (Guid.TryParse(id, out var idParseado) && idParseado != Guid.Empty)
        {
            var eliminado = await _mockQuestionService.Object.DeleteAsync(id);
            return eliminado ? new OkObjectResult("ID de pregunta válido") : new BadRequestObjectResult("Error al eliminar la pregunta");
        }
        else
        {
            return new BadRequestObjectResult("ID de pregunta inválido");
        }
    }

    /// <summary>
    /// Verifica que el resultado OK contenga los datos esperados.
    /// </summary>
    /// <param name="resultado">El resultado de la acción.</param>
    /// <param name="resultadoEsperado">Los datos esperados.</param>
    private void AssertOkResultConDatosEsperados(IActionResult resultado, dynamic resultadoEsperado)
    {
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        dynamic resultadoActual = okResult.Value;
        Assert.Equal(resultadoEsperado.Pregunta, resultadoActual.Pregunta);
    }

    /// <summary>
    /// Verifica que el resultado BadRequest sea para un ID inválido.
    /// </summary>
    /// <param name="resultado">El resultado de la acción.</param>
    private void AssertBadRequestParaIdInvalido(IActionResult resultado)
    {
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.Equal("ID de pregunta inválido", badRequestResult.Value);
    }
}