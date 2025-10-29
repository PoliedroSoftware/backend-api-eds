# Integration Testing Examples

This document provides additional examples for writing integration tests with WireMock and xUnit.

## Example 1: Testing a Simple HTTP GET Request

```csharp
using System.Net;
using System.Text.Json;
using Poliedro.Eds.Api.Tests.Infrastructure;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Examples;

public class SimpleHttpGetTests : IClassFixture<WireMockFixture>
{
    private readonly WireMockFixture _wireMockFixture;

    public SimpleHttpGetTests(WireMockFixture wireMockFixture)
    {
        _wireMockFixture = wireMockFixture;
    }

    [Fact]
    public async Task GetData_WhenServiceReturnsSuccess_ShouldReturnData()
    {
        // Arrange
        var mockServer = _wireMockFixture.Server;
        mockServer.Reset();

        var expectedData = new { id = 1, name = "Test Item" };

        mockServer
            .Given(Request.Create()
                .WithPath("/api/items/1")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(expectedData)));

        var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

        // Act
        var response = await httpClient.GetAsync("/api/items/1");
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
    }
}
```

## Example 2: Testing a POST Request with Request Body Validation

```csharp
[Fact]
public async Task PostData_WithValidPayload_ShouldReturnCreated()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    var requestPayload = new { name = "New Item", description = "Test Description" };

    mockServer
        .Given(Request.Create()
            .WithPath("/api/items")
            .UsingPost()
            .WithBody(new JsonMatcher(JsonSerializer.Serialize(requestPayload))))
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Created)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonSerializer.Serialize(new { id = 123 })));

    var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

    // Act
    var content = new StringContent(
        JsonSerializer.Serialize(requestPayload),
        System.Text.Encoding.UTF8,
        "application/json");
    var response = await httpClient.PostAsync("/api/items", content);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
}
```

## Example 3: Testing with Headers

```csharp
[Fact]
public async Task GetProtectedResource_WithAuthToken_ShouldReturnData()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    mockServer
        .Given(Request.Create()
            .WithPath("/api/protected")
            .UsingGet()
            .WithHeader("Authorization", "Bearer test-token"))
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBody("Protected data"));

    var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

    // Act
    var response = await httpClient.GetAsync("/api/protected");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

## Example 4: Testing Error Scenarios

```csharp
[Fact]
public async Task GetData_WhenServiceReturnsNotFound_ShouldHandleError()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    mockServer
        .Given(Request.Create()
            .WithPath("/api/items/999")
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound)
            .WithBody("Item not found"));

    var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

    // Act
    var response = await httpClient.GetAsync("/api/items/999");

    // Assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
}
```

## Example 5: Testing with Query Parameters

```csharp
[Fact]
public async Task SearchItems_WithQueryParams_ShouldReturnFilteredResults()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    mockServer
        .Given(Request.Create()
            .WithPath("/api/items")
            .WithParam("category", "electronics")
            .WithParam("maxPrice", "100")
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonSerializer.Serialize(new[] 
            { 
                new { id = 1, name = "Phone" },
                new { id = 2, name = "Tablet" }
            })));

    var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };

    // Act
    var response = await httpClient.GetAsync("/api/items?category=electronics&maxPrice=100");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

## Example 6: Using WireMock Request Verification

```csharp
[Fact]
public async Task UpdateItem_ShouldMakeCorrectRequest()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    mockServer
        .Given(Request.Create()
            .WithPath("/api/items/1")
            .UsingPut())
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NoContent));

    var httpClient = new HttpClient { BaseAddress = new Uri(mockServer.Url!) };
    var updateData = new { name = "Updated Item" };

    // Act
    var content = new StringContent(
        JsonSerializer.Serialize(updateData),
        System.Text.Encoding.UTF8,
        "application/json");
    await httpClient.PutAsync("/api/items/1", content);

    // Assert - Verify the request was made
    var logEntries = mockServer.LogEntries;
    Assert.Single(logEntries);
    Assert.Equal("/api/items/1", logEntries.First().RequestMessage.Path);
    Assert.Equal("PUT", logEntries.First().RequestMessage.Method);
}
```

## Example 7: Testing Timeouts and Delays

```csharp
[Fact]
public async Task GetData_WhenServiceIsSlowButWithinTimeout_ShouldSucceed()
{
    // Arrange
    var mockServer = _wireMockFixture.Server;
    mockServer.Reset();

    mockServer
        .Given(Request.Create()
            .WithPath("/api/slow")
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBody("Delayed response")
            .WithDelay(TimeSpan.FromMilliseconds(100)));

    var httpClient = new HttpClient 
    { 
        BaseAddress = new Uri(mockServer.Url!),
        Timeout = TimeSpan.FromSeconds(5)
    };

    // Act
    var response = await httpClient.GetAsync("/api/slow");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

## Tips for Writing Effective Integration Tests

1. **Always Reset the Mock Server** - Call `mockServer.Reset()` at the start of each test
2. **Use Descriptive Test Names** - Follow the pattern: `MethodName_Condition_ExpectedResult`
3. **Test Both Success and Failure Scenarios** - Don't just test happy paths
4. **Verify Request Details** - Check headers, query params, and request bodies when appropriate
5. **Keep Tests Independent** - Each test should run in isolation
6. **Use Realistic Test Data** - Test data should resemble production data
7. **Clean Up Resources** - Use fixtures and `IDisposable` to clean up resources

## Common Patterns

### Pattern 1: AAA (Arrange-Act-Assert)
Always structure tests with clear sections:
- **Arrange**: Set up mocks and test data
- **Act**: Execute the code under test
- **Assert**: Verify the results

### Pattern 2: Fixture Reuse
Use `IClassFixture<T>` to share expensive setup across tests in a class.

### Pattern 3: Multiple Scenarios
Group related tests in the same test class to organize tests logically.

## Running Specific Examples

To run only the example tests:
```bash
dotnet test --filter "FullyQualifiedName~Examples"
```
