using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using Poliedro.Eds.Domain.Islander.DomainIslander;

namespace Poliedro.Eds.Infraestructure.External.Keycloak;

public static class DependencyInjectionService
{
    public static IServiceCollection AddKeycloakServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IKeycloakUserService, KeycloakService>();
        return services;
    }
}
