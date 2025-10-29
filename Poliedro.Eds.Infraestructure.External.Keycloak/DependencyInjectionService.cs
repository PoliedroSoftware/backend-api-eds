using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Domain.Auth.DomainAuth;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;

namespace Poliedro.Eds.Infraestructure.External.Keycloak;

public static class DependencyInjectionService
{
    public static IServiceCollection AddKeycloakServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IKeycloakUserService, KeycloakService>();
        services.AddHttpClient<IKeycloakAuthService, KeycloakAuthService>();
        return services;
    }
}
