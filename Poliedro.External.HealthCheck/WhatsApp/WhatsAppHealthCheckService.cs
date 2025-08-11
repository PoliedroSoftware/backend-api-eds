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
            
            // Validate URL format
            if (!Uri.TryCreate(whatsAppUrl, UriKind.Absolute, out var uri))
            {
                return HealthCheckResult.Unhealthy("WhatsApp URL configuration is invalid.");
            }
            
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsAppToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // For WhatsApp Business API, we'll make a simple connectivity check
            // Using a HEAD request to the base domain to verify network connectivity
            var baseUrl = $"{uri.Scheme}://{uri.Host}";
            var response = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, baseUrl), 
                cancellationToken);
            
            // For WhatsApp API, even a 404 or other non-success status from the base domain 
            // indicates that we can reach the host, which is sufficient for health check
            return response.StatusCode != System.Net.HttpStatusCode.RequestTimeout && 
                   response.StatusCode != System.Net.HttpStatusCode.ServiceUnavailable
                ? HealthCheckResult.Healthy("WhatsApp API is reachable.")
                : HealthCheckResult.Unhealthy($"WhatsApp API returned status code: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy($"WhatsApp API connectivity failed: {ex.Message}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return HealthCheckResult.Unhealthy("WhatsApp API health check timed out.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"WhatsApp health check failed: {ex.Message}");
        }
    }
}