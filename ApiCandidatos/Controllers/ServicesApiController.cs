#region Documentación
/****************************************************************************************************
* WEBAPI                                                      
****************************************************************************************************
* Unidad        : <.NET/C# controller por defecto ServicesApi>                                                                      
* DescripciÓn   : <Logica de negocio para el controller por defecto ServicesApi>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <03-09-2024>                                                                             
***************************************************************************************************/
#endregion Documentación

using Web.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Endpoints.Services;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ServicesApiController : ControllerBase
    {
        IServicesMiniApi iServicesApi;
        WebApiContext dbcontext;

        /// Manejo de logs
        private readonly ILogger<ServicesApiController> _logger;

        public ServicesApiController(IServicesMiniApi services, ILogger<ServicesApiController> loggerServices, WebApiContext db)
        {
            _logger = loggerServices;

            iServicesApi = services;
            dbcontext = db;
        }

        /// Creacion de base de datos
        [HttpGet]
        [Route("createdb")]
        public IActionResult CreateDataBase()
        {
            dbcontext.Database.EnsureCreated();

            return Ok();
        }
    }
}
