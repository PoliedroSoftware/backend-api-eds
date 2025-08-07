using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Poliedro.External.HealthCheck.Keycloak;

public class KeycloakHealthCheckService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("KeycloakHealthCheck");
            var keycloakUri = configuration["Keycloak:KeycloakUri"];
            
            if (string.IsNullOrWhiteSpace(keycloakUri))
            {
                return HealthCheckResult.Unhealthy("Keycloak URI is not configured.");
            }

            // Use the OpenID Connect discovery endpoint to check if Keycloak is responding
            var realm = configuration["Keycloak:Realm"] ?? "AppEDS";
            var healthCheckUrl = $"{keycloakUri}/realms/{realm}/.well-known/openid_configuration";
            
            var response = await httpClient.GetAsync(healthCheckUrl, cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Keycloak service is healthy.")
                : HealthCheckResult.Unhealthy($"Keycloak service is not healthy. Status: {response.StatusCode}");

        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Keycloak health check failed: {ex.Message}");
        }
    }
}