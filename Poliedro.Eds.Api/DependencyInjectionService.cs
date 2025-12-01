using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Poliedro.Eds.Api.Common.Configurations;

namespace Poliedro.Eds.Api;

public static class DependencyInjectionService
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddFluentValidationServices();

        // Configure OpenAPI with Microsoft.AspNetCore.OpenApi
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Poliedro Eds API",
                    Description = "Administrator de APIs for Eds"
                };
                return Task.CompletedTask;
            });

            // Add JWT Bearer authentication security scheme
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();

                if (document.Components.SecuritySchemes == null)
                {
                    document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
                }

                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
                };

                return Task.CompletedTask;
            });

            // Add security requirement to all operations
            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                var securityRequirement = new OpenApiSecurityRequirement();
                var securitySchemeReference = new OpenApiSecuritySchemeReference("Bearer", null);
                securityRequirement.Add(securitySchemeReference, new List<string>());

                operation.Security.Add(securityRequirement);
                return Task.CompletedTask;
            });
        });

        return services;
    }
}