namespace ESTMS.API.Host.Capabilities
{
    public static class StartupCors
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowVueFrontend", builder =>
                {
                    builder.WithOrigins("https://betterteamwins.netlify.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
