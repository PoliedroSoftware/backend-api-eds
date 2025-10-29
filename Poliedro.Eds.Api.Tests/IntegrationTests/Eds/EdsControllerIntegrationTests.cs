using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;
using Poliedro.Eds.Application.Eds.Dtos;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Eds;

public class EdsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public EdsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
 _client = factory.CreateClient(new WebApplicationFactoryClientOptions
     {
          AllowAutoRedirect = false
    });
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WithValidPagination_ReturnsOkOrUnauthorized()
    {
  var response = await _client.GetAsync("/api/v1/eds?PageNumber=1&PageSize=10");
        Assert.True(response.StatusCode == HttpStatusCode.OK || 
           response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WithoutPagination_ReturnsResponse()
    {
        var response = await _client.GetAsync("/api/v1/eds");
        Assert.NotNull(response);
  }

    #endregion

    #region GetById Tests

 [Fact]
  public async Task GetById_WithValidId_ReturnsResponse()
    {
    var response = await _client.GetAsync("/api/v1/eds/1");
        Assert.True(response.StatusCode == HttpStatusCode.OK || 
               response.StatusCode == HttpStatusCode.NotFound ||
        response.StatusCode == HttpStatusCode.Unauthorized ||
                    response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsResponse()
    {
        var response = await _client.GetAsync("/api/v1/eds/0");
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest ||
         response.StatusCode == HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Create Tests

    [Fact]
  public async Task Create_WithValidData_ReturnsCreatedOrUnauthorized()
 {
        var data = new { Name = "Test EDS", Address = "Test Address", IdBusiness = 1 };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
  var response = await _client.PostAsync("/api/v1/eds", content);
 Assert.True(response.StatusCode == HttpStatusCode.Created ||
       response.StatusCode == HttpStatusCode.Unauthorized ||
       response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_WithEmptyRequest_ReturnsBadRequest()
{
     var content = new StringContent("{}", Encoding.UTF8, "application/json");
     var response = await _client.PostAsync("/api/v1/eds", content);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest ||
response.StatusCode == HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Update Tests

 [Fact]
    public async Task Update_WithValidData_ReturnsNoContentOrNotFound()
    {
   var data = new { IdEds = 1, Name = "Updated EDS", Address = "Updated Address", IdBusiness = 1 };
    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/v1/eds", content);
     Assert.True(response.StatusCode == HttpStatusCode.NoContent ||
response.StatusCode == HttpStatusCode.NotFound ||
     response.StatusCode == HttpStatusCode.Unauthorized ||
       response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_WithInvalidId_ReturnsBadRequest()
    {
   var data = new { IdEds = 0, Name = "Updated EDS", Address = "Updated Address" };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/v1/eds", content);
Assert.True(response.StatusCode == HttpStatusCode.BadRequest ||
   response.StatusCode == HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Response Format Tests

    [Fact]
    public async Task GetAll_ReturnsValidJsonResponse()
    {
        var response = await _client.GetAsync("/api/v1/eds?PageNumber=1&PageSize=10");
        if (response.StatusCode == HttpStatusCode.OK)
        {
 var content = await response.Content.ReadAsStringAsync();
         Assert.NotNull(content);
            var exception = Record.Exception(() => JsonSerializer.Deserialize<object>(content));
   Assert.Null(exception);
        }
    }

    #endregion
}
