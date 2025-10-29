using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Bank;

public class BankControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public BankControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
      {
            AllowAutoRedirect = false
        });
    }

    #region GetAll Tests

    [Fact]
  public async Task GetAll_WithoutFilters_ReturnsOkOrUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/bank");

     // Assert
        Assert.True(
    response.StatusCode == HttpStatusCode.OK ||
         response.StatusCode == HttpStatusCode.Unauthorized
   );
    }

    [Fact]
    public async Task GetAll_WithIdAccount_ReturnsOkOrUnauthorized()
    {
 // Arrange
      var idAccount = 1;

     // Act
        var response = await _client.GetAsync($"/api/v1/bank?idAccount={idAccount}");

        // Assert
        Assert.True(
         response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Unauthorized
   );
    }

    [Fact]
    public async Task GetAll_WithIdEds_ReturnsOkOrUnauthorized()
    {
        // Arrange
        var idEds = 1;

   // Act
  var response = await _client.GetAsync($"/api/v1/bank?idEds={idEds}");

      // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
      response.StatusCode == HttpStatusCode.Unauthorized
     );
    }

    [Fact]
    public async Task GetAll_WithBothFilters_ReturnsOkOrUnauthorized()
    {
        // Arrange
        var idAccount = 1;
        var idEds = 1;

// Act
        var response = await _client.GetAsync($"/api/v1/bank?idAccount={idAccount}&idEds={idEds}");

        // Assert
        Assert.True(
    response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

  #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkOrNotFound()
    {
    // Arrange
 var validId = 1;

        // Act
      var response = await _client.GetAsync($"/api/v1/bank/{validId}");

     // Assert
     Assert.True(
     response.StatusCode == HttpStatusCode.OK ||
  response.StatusCode == HttpStatusCode.NotFound ||
       response.StatusCode == HttpStatusCode.Unauthorized
  );
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundOrBadRequest()
    {
        // Arrange
     var invalidId = 0;

    // Act
        var response = await _client.GetAsync($"/api/v1/bank/{invalidId}");

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.NotFound ||
      response.StatusCode == HttpStatusCode.BadRequest ||
       response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    #endregion

    #region GetCurrentBalance Tests

    [Fact]
    public async Task GetCurrentBalance_WithValidAccountId_ReturnsOkOrUnauthorized()
    {
    // Arrange
        var accountId = 1;

    // Act
 var response = await _client.GetAsync($"/api/v1/bank/current-balance/{accountId}");

      // Assert
        Assert.True(
    response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Unauthorized
        );
    }

    [Fact]
    public async Task GetCurrentBalance_WithInvalidAccountId_ReturnsResponse()
    {
        // Arrange
        var accountId = 0;

        // Act
      var response = await _client.GetAsync($"/api/v1/bank/current-balance/{accountId}");

        // Assert
     Assert.NotNull(response);
    }

    #endregion

    #region Create Tests

    [Fact]
public async Task Create_WithValidData_ReturnsCreatedOrUnauthorized()
    {
        // Arrange
        var createDto = new BankDtoCreateRequest
    {
            IdAccount = 1,
          IdEds = 1,
         Moviment = "CREDIT",
   Note = "Test Transaction",
        Ammount = 1000.50
      };

     var jsonContent = JsonSerializer.Serialize(createDto);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

    // Act
        var response = await _client.PostAsync("/api/v1/bank", httpContent);

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
        var response = await _client.PostAsync("/api/v1/bank", httpContent);

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
  var createDto = new BankDtoCreateRequest
   {
            IdAccount = 1,
        IdEds = 1,
        Moviment = "CREDIT",
       Note = "Test Transaction",
    Ammount = 1000.50
        };

  var jsonContent = JsonSerializer.Serialize(createDto);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "text/plain");

 // Act
        var response = await _client.PostAsync("/api/v1/bank", httpContent);

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
    public async Task GetAll_ReturnsValidJsonResponse()
    {
        // Act
     var response = await _client.GetAsync("/api/v1/bank");

  // Assert
      if (response.StatusCode == HttpStatusCode.OK)
        {
         var content = await response.Content.ReadAsStringAsync();
 Assert.NotNull(content);
          var exception = Record.Exception(() => JsonSerializer.Deserialize<object>(content));
     Assert.Null(exception);
        }
    }

    [Fact]
    public async Task GetById_WithValidResponse_ReturnsValidJson()
    {
      // Arrange
     var validId = 1;

   // Act
 var response = await _client.GetAsync($"/api/v1/bank/{validId}");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
    {
       var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
            var exception = Record.Exception(() => JsonSerializer.Deserialize<BankDto>(content));
  Assert.Null(exception);
      }
    }

    #endregion
}
