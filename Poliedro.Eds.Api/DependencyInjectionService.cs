using Microsoft.OpenApi.Models;
using Poliedro.Eds.Api.Common.Configurations;

namespace Poliedro.Eds.Api;

public static class DependencyInjectionService
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {

        services.AddFluentValidationServices();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Poliedro Eds API",
                Description = "Administrator de APIs for Eds "
            });

            options.EnableAnnotations();

            var basePath = AppContext.BaseDirectory;

        });
        return services;
    }
}

