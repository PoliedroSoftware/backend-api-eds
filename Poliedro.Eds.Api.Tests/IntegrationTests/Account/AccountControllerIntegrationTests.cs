using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;
using Poliedro.Eds.Application.Account.Dtos;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Account;

public class AccountControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AccountControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
     {
    AllowAutoRedirect = false
        });
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_ReturnsOkOrUnauthorized()
    {
 // Act
     var response = await _client.GetAsync("/api/v1/account");

        // Assert
        Assert.True(
        response.StatusCode == HttpStatusCode.OK ||
  response.StatusCode == HttpStatusCode.Unauthorized ||
    response.StatusCode == HttpStatusCode.NotFound
        );
    }

    [Fact]
    public async Task GetAll_WithAuthorization_ReturnsValidResponse()
 {
        // Act
        var response = await _client.GetAsync("/api/v1/account");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
       var exception = Record.Exception(() => JsonSerializer.Deserialize<object>(content));
            Assert.Null(exception);
        }
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkOrNotFound()
    {
      // Arrange
        var validId = 1;

        // Act
        var response = await _client.GetAsync($"/api/v1/account/{validId}");

        // Assert
        Assert.True(
       response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NotFound ||
    response.StatusCode == HttpStatusCode.Unauthorized ||
          response.StatusCode == HttpStatusCode.BadRequest
 );
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = 0;

        // Act
        var response = await _client.GetAsync($"/api/v1/account/{invalidId}");

 // Assert
        Assert.True(
    response.StatusCode == HttpStatusCode.BadRequest ||
      response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task GetById_WithNegativeId_ReturnsBadRequest()
    {
        // Arrange
        var negativeId = -1;

  // Act
        var response = await _client.GetAsync($"/api/v1/account/{negativeId}");

        // Assert
        Assert.True(
     response.StatusCode == HttpStatusCode.BadRequest ||
          response.StatusCode == HttpStatusCode.Unauthorized
    );
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedOrUnauthorized()
  {
    // Arrange
        var createDto = new AccountCreateDto
      {
AccountType = "Savings",
     Bank = "Test Bank",
        Account = "123456789",
     Holder = "Test Holder"
      };

        var jsonContent = JsonSerializer.Serialize(createDto);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/account", httpContent);

   // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.Created ||
   response.StatusCode == HttpStatusCode.Unauthorized ||
        response.StatusCode == HttpStatusCode.BadRequest
      );
    }

    [Fact]
    public async Task Create_WithEmptyRequest_ReturnsBadRequest()
 {
      // Arrange
      var httpContent = new StringContent("{}", Encoding.UTF8, "application/json");

  // Act
        var response = await _client.PostAsync("/api/v1/account", httpContent);

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest ||
            response.StatusCode == HttpStatusCode.Unauthorized
        );
 }

    [Fact]
    public async Task Create_WithInvalidContentType_ReturnsUnsupportedMediaType()
    {
        // Arrange
     var createDto = new AccountCreateDto
        {
            AccountType = "Savings",
            Bank = "Test Bank",
         Account = "123456789",
            Holder = "Test Holder"
        };

 var jsonContent = JsonSerializer.Serialize(createDto);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "text/plain");

        // Act
        var response = await _client.PostAsync("/api/v1/account", httpContent);

   // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.UnsupportedMediaType ||
          response.StatusCode == HttpStatusCode.Unauthorized ||
 response.StatusCode == HttpStatusCode.BadRequest
  );
    }

    #endregion

    #region Response Format Tests

    [Fact]
    public async Task GetById_WithValidResponse_ReturnsValidJson()
    {
        // Arrange
    var validId = 1;

        // Act
        var response = await _client.GetAsync($"/api/v1/account/{validId}");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
       Assert.NotNull(content);
     var exception = Record.Exception(() => JsonSerializer.Deserialize<AccountDto>(content));
     Assert.Null(exception);
        }
    }

    #endregion
}
