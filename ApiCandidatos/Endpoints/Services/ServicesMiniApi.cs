#region Documentación
/****************************************************************************************************
* WEBAPI                                                      
****************************************************************************************************
* Unidad        : <.NET/C# inyeccion de dependencias>                                                                      
* DescripciÓn   : <Logica de negocio para realizar la inyeccion de dependencias>                                                      
* Autor         : <Pedro Castro>
* Fecha         : <03-09-2024>    
* ===========   ============       ========================================================================= 
***************************************************************************************************/
#endregion Documentación

namespace Web.Api.Endpoints.Services
{
    public class ServicesMiniApi : IServicesMiniApi
    {
        public string GetApiDocumentacion()
        {
            return "API para la inyeccion de dependencias";
        }
    }
    public interface IServicesMiniApi
    {
        string GetApiDocumentacion();
    }
}
