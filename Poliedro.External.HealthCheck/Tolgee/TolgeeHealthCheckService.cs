using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Poliedro.External.HealthCheck.Tolgee;

public class TolgeeHealthCheckService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("TranslationCachingService");
            var response = await httpClient.GetAsync("translations?size=1");
          
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Tolgee API is healthy.")
                : HealthCheckResult.Unhealthy("Tolgee API is not healthy.");
            
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Tolgee health check failed: {ex.Message}");
        }
    }
}

