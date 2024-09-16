namespace Web.Api.Extensions;

public static class CorsExtensions{

    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        //Aquí deberá implementar la lógica para configurar CORS

        return services;
    }
}