# Integration Tests with WireMock and xUnit

This document describes the integration testing infrastructure for the Poliedro EDS Backend API using WireMock.Net and xUnit.

## Overview

The test project (`Poliedro.Eds.Api.Tests`) contains integration tests that use **WireMock.Net** to mock external HTTP services and **xUnit** as the testing framework.

## Key Components

### 1. WireMock Fixture

**File**: `Infrastructure/WireMockFixture.cs`

The `WireMockFixture` provides a reusable WireMock server instance that can be shared across multiple tests using xUnit's `IClassFixture` pattern.

```csharp
public class WireMockFixture : IDisposable
{
    public WireMockServer Server { get; }

    public WireMockFixture()
    {
        Server = WireMockServer.Start();
    }

    public void Dispose()
    {
        Server?.Stop();
        Server?.Dispose();
    }
}
```

### 2. Custom Web Application Factory

**File**: `Infrastructure/CustomWebApplicationFactory.cs`

The `CustomWebApplicationFactory` extends `WebApplicationFactory<Program>` to provide a customized test environment for API integration tests.

### 3. Integration Tests

**Directory**: `IntegrationTests/`

#### Keycloak Service Integration Tests

**File**: `IntegrationTests/KeycloakServiceIntegrationTests.cs`

Tests for the Keycloak user management service using WireMock to simulate Keycloak API responses.

**Example tests:**
- ✅ `CreateUserAsync_WhenKeycloakReturnsSuccess_ShouldReturnSuccess` - Tests successful user creation
- ✅ `CreateUserAsync_WhenTokenRequestFails_ShouldReturnError` - Tests authentication failure handling
- ✅ `CreateUserAsync_WhenCreateUserFails_ShouldReturnError` - Tests user creation conflict handling

## Dependencies

The following NuGet packages are required for integration testing:

- `xunit` (2.5.3) - Testing framework
- `xunit.runner.visualstudio` (2.5.3) - Test runner
- `WireMock.Net` (1.15.0) - HTTP mocking library
- `Microsoft.AspNetCore.Mvc.Testing` (8.0.11) - ASP.NET Core testing infrastructure
- `Microsoft.NET.Test.Sdk` (17.8.0) - Test SDK

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run only integration tests
```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Run tests for a specific service
```bash
dotnet test --filter "FullyQualifiedName~KeycloakServiceIntegrationTests"
```

### Run with verbose output
```bash
dotnet test --verbosity detailed
```

## Writing New Integration Tests

### 1. Testing External HTTP Services

To test a service that makes HTTP calls:

1. Create a test class that implements `IClassFixture<WireMockFixture>`
2. Configure WireMock to respond to expected HTTP requests
3. Create the service with an HttpClient pointing to the WireMock server
4. Execute the test and verify results

**Example:**

```csharp
public class MyServiceIntegrationTests : IClassFixture<WireMockFixture>
{
    private readonly WireMockFixture _wireMockFixture;

    public MyServiceIntegrationTests(WireMockFixture wireMockFixture)
    {
        _wireMockFixture = wireMockFixture;
    }

    [Fact]
    public async Task MyMethod_ShouldReturnExpectedResult()
    {
        // Arrange
        var mockServer = _wireMockFixture.Server;
        mockServer.Reset();

        mockServer
            .Given(Request.Create()
                .WithPath("/api/endpoint")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new { data = "test" })));

        var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };
        var service = new MyService(httpClient);

        // Act
        var result = await service.MyMethod();

        // Assert
        Assert.NotNull(result);
    }
}
```

### 2. Testing API Endpoints

To test API endpoints using the Web Application Factory:

1. Use `CustomWebApplicationFactory` to create a test server
2. Create an HTTP client from the factory
3. Make requests and verify responses

```csharp
public class ApiEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEndpoint_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/resource");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

## Best Practices

1. **Reset WireMock state** - Always call `mockServer.Reset()` at the beginning of each test
2. **Use specific paths** - Configure WireMock with exact paths to avoid conflicts
3. **Verify request details** - Use WireMock's verification features to ensure requests match expectations
4. **Clean up resources** - Use fixtures to properly dispose of resources
5. **Isolate tests** - Each test should be independent and not rely on others
6. **Mock external dependencies** - Never make real HTTP calls to external services in tests

## Troubleshooting

### Tests fail with connection errors
- Ensure WireMock server is properly started in the fixture
- Check that the HttpClient BaseAddress matches the WireMock server URL

### Tests hang or timeout
- Verify that all HTTP requests have corresponding WireMock configurations
- Check for infinite loops or missing async/await

### Version conflicts
- Ensure all package versions are compatible with .NET 8.0
- Check for transitive dependency conflicts

## Additional Resources

- [WireMock.Net Documentation](https://github.com/WireMock-Net/WireMock.Net/wiki)
- [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)
- [ASP.NET Core Integration Tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
