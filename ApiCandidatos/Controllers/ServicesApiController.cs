#region Documentación
/****************************************************************************************************
* WEBAPI                                                      
****************************************************************************************************
* Unidad        : ServicesApiController                                                                      
* Descripción   : Controlador que maneja las operaciones relacionadas con el servicio API. 
* Autor         : Pedro Castro
* Fecha         : 03-09-2024                                                                             
***************************************************************************************************/
#endregion Documentación

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Api.Context;
using Web.Api.Endpoints.Services;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesApiController : ControllerBase
    {
        private readonly IServicesMiniApi _servicesApi;
        private readonly WebApiContext _dbContext;
        private readonly ILogger<ServicesApiController> _logger;

        public ServicesApiController(IServicesMiniApi servicesApi, ILogger<ServicesApiController> logger, WebApiContext dbContext)
        {
            _servicesApi = servicesApi ?? throw new ArgumentNullException(nameof(servicesApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Crea la base de datos si no existe.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        [HttpGet("createdb")]
        public IActionResult CreateDataBase()
        {
            try
            {
                _dbContext.Database.EnsureCreated();
                _logger.LogInformation("Base de datos creada exitosamente.");
                return Ok("Base de datos creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la base de datos.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}