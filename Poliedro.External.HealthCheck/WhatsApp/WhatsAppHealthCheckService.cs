using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Poliedro.External.HealthCheck.WhatsApp;

public class WhatsAppHealthCheckService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            
            // Configure WhatsApp API client
            var whatsAppUrl = configuration["WhatsApp:Url"];
            var whatsAppToken = configuration["WhatsApp:Token"];
            
            if (string.IsNullOrWhiteSpace(whatsAppUrl))
            {
                return HealthCheckResult.Unhealthy("WhatsApp URL configuration is missing.");
            }
            
            if (string.IsNullOrWhiteSpace(whatsAppToken))
            {
                return HealthCheckResult.Unhealthy("WhatsApp Token configuration is missing.");
            }
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsAppToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Make a simple GET request to check if the API is accessible
            // Using the base URL to check connectivity without sending actual messages
            var baseUri = new Uri(whatsAppUrl);
            var healthCheckUrl = $"{baseUri.Scheme}://{baseUri.Host}";
            
            var response = await httpClient.GetAsync(healthCheckUrl, cancellationToken);
            
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("WhatsApp API is healthy.")
                : HealthCheckResult.Unhealthy($"WhatsApp API returned status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"WhatsApp health check failed: {ex.Message}");
        }
    }
}