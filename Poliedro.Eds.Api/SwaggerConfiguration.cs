using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Poliedro.Eds.Api;

/// <summary>
/// Swagger configuration extensions for Minimal APIs support
/// </summary>
public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwaggerForMinimalApis(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Poliedro Eds API",
                Description = "Administrator de APIs for Eds"
            });

            options.EnableAnnotations();
            
            // Include all endpoints (Minimal APIs and Controllers)
            options.DocInclusionPredicate((docName, apiDesc) => true);

            // JWT Bearer authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            // Better handling of generic types
            options.CustomSchemaIds(type =>
            {
                if (type.IsGenericType)
                {
                    var genericTypeName = type.GetGenericTypeDefinition().Name.Split('`')[0];
                    var genericArgs = string.Join("_", type.GetGenericArguments().Select(t => t.Name));
                    return $"{genericTypeName}_{genericArgs}";
                }
                return type.FullName?.Replace("+", "_") ?? type.Name;
            });

            // Resolve conflicting actions by taking the first one
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            // Schema filter to clean up problematic types
            options.SchemaFilter<CleanSchemaFilter>();
        });

        return services;
    }
}

/// <summary>
/// Schema filter to remove problematic properties from Swagger documentation
/// </summary>
public class CleanSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        // Remove obsolete properties
        var obsoleteProperties = context.Type.GetProperties()
            .Where(t => t.GetCustomAttribute<ObsoleteAttribute>() != null)
            .Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1))
            .ToList();

        foreach (var propertyName in obsoleteProperties)
        {
            if (schema.Properties.ContainsKey(propertyName))
                schema.Properties.Remove(propertyName);
        }
    }
}
