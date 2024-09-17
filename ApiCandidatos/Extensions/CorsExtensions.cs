namespace Web.Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.WithOrigins("https://localhost:7241/") // Reemplaza con los orígenes permitidos
                           .WithMethods("GET", "POST", "DELETE") // Métodos HTTP permitidos
                           .WithHeaders("Content-Type", "Authorization"); // Encabezados permitidos
                });
            });

            return services;
        }
    }
}