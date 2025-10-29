using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Api.Tests.Infrastructure;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Poliedro.Eds.Api.Tests.IntegrationTests;

public class KeycloakServiceIntegrationTests : IClassFixture<WireMockFixture>
{
    private readonly WireMockFixture _wireMockFixture;

    public KeycloakServiceIntegrationTests(WireMockFixture wireMockFixture)
    {
        _wireMockFixture = wireMockFixture;
    }

    [Fact]
    public async Task CreateUserAsync_WhenKeycloakReturnsSuccess_ShouldReturnSuccess()
    {
        // Arrange
        var mockServer = _wireMockFixture.Server;
        mockServer.Reset();

        // Mock the token endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/realms/AppEDS/protocol/openid-connect/token")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new { access_token = "mock-token" })));

        // Mock the create user endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/admin/realms/TestRealm/users")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeader("Location", $"{mockServer.Url}/admin/realms/TestRealm/users/test-user-id"));

        // Mock the reset password endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/admin/realms/TestRealm/users/test-user-id/reset-password")
                .UsingPut())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.NoContent));

        // Mock the subgroups endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/admin/realms/TestRealm/groups/test-group-id/children")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new[]
                {
                    new { id = "subgroup-id", name = "TestGroup" }
                })));

        // Mock the assign group endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/admin/realms/TestRealm/users/test-user-id/groups/subgroup-id")
                .UsingPut())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.NoContent));

        // Create configuration
        var configValues = new Dictionary<string, string>
        {
            { "Keycloak:KeycloakUri", mockServer.Url },
            { "Keycloak:Realm", "TestRealm" },
            { "Keycloak:DefaultGroupId", "test-group-id" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        // Create HTTP client
        var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

        // Create service instance
        var service = new KeycloakService(httpClient, configuration, null!);

        // Create test islander
        var islander = new IslanderEntity
        {
            IdEds = 12345,
            Name = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var result = await service.CreateUserAsync(islander, "password123", "TestGroup");

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateUserAsync_WhenTokenRequestFails_ShouldReturnError()
    {
        // Arrange
        var mockServer = _wireMockFixture.Server;
        mockServer.Reset();

        // Mock the token endpoint to fail
        mockServer
            .Given(Request.Create()
                .WithPath("/realms/AppEDS/protocol/openid-connect/token")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.Unauthorized));

        // Create configuration
        var configValues = new Dictionary<string, string>
        {
            { "Keycloak:KeycloakUri", mockServer.Url },
            { "Keycloak:Realm", "TestRealm" },
            { "Keycloak:DefaultGroupId", "test-group-id" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        // Create HTTP client
        var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

        // Create service instance
        var service = new KeycloakService(httpClient, configuration, null!);

        // Create test islander
        var islander = new IslanderEntity
        {
            IdEds = 12345,
            Name = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var result = await service.CreateUserAsync(islander, "password123", "TestGroup");

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task CreateUserAsync_WhenCreateUserFails_ShouldReturnError()
    {
        // Arrange
        var mockServer = _wireMockFixture.Server;
        mockServer.Reset();

        // Mock the token endpoint
        mockServer
            .Given(Request.Create()
                .WithPath("/realms/AppEDS/protocol/openid-connect/token")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new { access_token = "mock-token" })));

        // Mock the create user endpoint to fail
        mockServer
            .Given(Request.Create()
                .WithPath("/admin/realms/TestRealm/users")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.Conflict)
                .WithBody("User already exists"));

        // Create configuration
        var configValues = new Dictionary<string, string>
        {
            { "Keycloak:KeycloakUri", mockServer.Url },
            { "Keycloak:Realm", "TestRealm" },
            { "Keycloak:DefaultGroupId", "test-group-id" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        // Create HTTP client
        var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

        // Create service instance
        var service = new KeycloakService(httpClient, configuration, null!);

        // Create test islander
        var islander = new IslanderEntity
        {
            IdEds = 12345,
            Name = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var result = await service.CreateUserAsync(islander, "password123", "TestGroup");

        // Assert
        Assert.False(result.IsSuccess);
    }
}
