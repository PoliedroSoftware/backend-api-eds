using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Moq.Protected;
using Poliedro.External.HealthCheck.Keycloak;

namespace Poliedro.Eds.Api.Tests;

public class KeycloakHealthCheckServiceTests
{
    [Fact]
    public async Task CheckHealthAsync_WhenKeycloakRespondsSuccessfully_ReturnsHealthy()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient("KeycloakHealthCheck")).Returns(httpClient);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:KeycloakUri"] = "https://keycloak.example.com",
                ["Keycloak:Realm"] = "TestRealm"
            })
            .Build();

        var healthCheckService = new KeycloakHealthCheckService(mockHttpClientFactory.Object, configuration);

        // Act
        var result = await healthCheckService.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Equal("Keycloak service is healthy.", result.Description);
    }

    [Fact]
    public async Task CheckHealthAsync_WhenKeycloakUriNotConfigured_ReturnsUnhealthy()
    {
        // Arrange
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var configuration = new ConfigurationBuilder().Build();

        var healthCheckService = new KeycloakHealthCheckService(mockHttpClientFactory.Object, configuration);

        // Act
        var result = await healthCheckService.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Equal("Keycloak URI is not configured.", result.Description);
    }

    [Fact]
    public async Task CheckHealthAsync_WhenKeycloakReturnsError_ReturnsUnhealthy()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient("KeycloakHealthCheck")).Returns(httpClient);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:KeycloakUri"] = "https://keycloak.example.com",
                ["Keycloak:Realm"] = "TestRealm"
            })
            .Build();

        var healthCheckService = new KeycloakHealthCheckService(mockHttpClientFactory.Object, configuration);

        // Act
        var result = await healthCheckService.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Keycloak service is not healthy", result.Description);
    }

    [Fact]
    public async Task CheckHealthAsync_WhenExceptionThrown_ReturnsUnhealthy()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(f => f.CreateClient("KeycloakHealthCheck")).Returns(httpClient);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:KeycloakUri"] = "https://keycloak.example.com",
                ["Keycloak:Realm"] = "TestRealm"
            })
            .Build();

        var healthCheckService = new KeycloakHealthCheckService(mockHttpClientFactory.Object, configuration);

        // Act
        var result = await healthCheckService.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Keycloak health check failed", result.Description);
    }
}