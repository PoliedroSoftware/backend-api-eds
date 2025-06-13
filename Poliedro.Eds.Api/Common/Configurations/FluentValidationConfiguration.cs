using FluentValidation.AspNetCore;
using FluentValidation;

namespace Poliedro.Eds.Api.Common.Configurations;

public static class FluentValidationConfiguration
{
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }

        return services;
    }
}
