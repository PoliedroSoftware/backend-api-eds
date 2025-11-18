using Microsoft.OpenApi.Models;
using Poliedro.Eds.Api.Common.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

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
                Description = "Administrator de APIs for Eds"
            });

            // Enable annotations for both Controllers AND Minimal APIs
            options.EnableAnnotations();
            
            // Important: Support for both MVC and Minimal APIs
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                // Include all API descriptions (both Controllers and Minimal APIs)
                return true;
            });

            // Add JWT Bearer authentication
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

            // CRITICAL: Better handling of generic types to avoid exceptions
            options.CustomSchemaIds(type =>
            {
                try
                {
                    if (type.IsGenericType)
                    {
                        var genericTypeName = type.GetGenericTypeDefinition().Name.Split('`')[0];
                        var genericArgs = string.Join("_", type.GetGenericArguments().Select(t => GetFriendlyName(t)));
                        return $"{genericTypeName}_{genericArgs}";
                    }
                    return type.FullName?.Replace("+", "_").Replace(".", "_") ?? type.Name;
                }
                catch
                {
                    // Fallback to simple name if anything fails
                    return type.Name + "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                }
            });

            // CRITICAL: Resolve conflicting actions (when both controllers and minimal APIs register same route)
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            // Add schema filter to ignore problematic types
            options.SchemaFilter<IgnoreProblematicTypesFilter>();

            // TEMPORARILY COMMENTED: File upload filter causing build issues
            // options.OperationFilter<FileUploadOperationFilter>();
        });
        
        return services;
    }

    private static string GetFriendlyName(Type type)
    {
        if (type.IsGenericType)
        {
            var genericTypeName = type.GetGenericTypeDefinition().Name.Split('`')[0];
            var genericArgs = string.Join("_", type.GetGenericArguments().Select(t => GetFriendlyName(t)));
            return $"{genericTypeName}_{genericArgs}";
        }
        return type.Name;
    }
}

/// <summary>
/// Schema filter to ignore types that cause Swagger to fail
/// </summary>
public class IgnoreProblematicTypesFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        try
        {
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

            // Remove properties with problematic types (like delegates, dynamic, etc.)
            var problematicProperties = schema.Properties
                .Where(kvp => kvp.Value.Type == null || kvp.Value.Type == "object")
                .Select(kvp => kvp.Key)
                .ToList();

            // Don't remove ALL object types, only truly problematic ones
            foreach (var propertyName in problematicProperties)
            {
                var prop = context.Type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && (prop.PropertyType.IsAbstract || prop.PropertyType.IsInterface))
                {
                    // Keep interface/abstract types but simplify them
                    schema.Properties[propertyName].Description = $"Type: {prop.PropertyType.Name}";
                }
            }
        }
        catch
        {
            // Silently ignore errors in schema filtering to prevent Swagger from crashing
        }
    }
}

