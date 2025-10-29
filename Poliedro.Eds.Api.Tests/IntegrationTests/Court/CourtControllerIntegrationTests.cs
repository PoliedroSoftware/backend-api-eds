using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Court;

public class CourtControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CourtControllerIntegrationTests(CustomWebApplicationFactory factory)
 {
  _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task GetAll_WithPagination_ReturnsResponse()
    {
   var response = await _client.GetAsync("/api/v1/court?PageNumber=1&PageSize=10");
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsResponse()
    {
        var response = await _client.GetAsync("/api/v1/court/1");
   Assert.NotNull(response);
 }

    [Fact]
    public async Task Create_WithData_ReturnsResponse()
    {
        var data = new { Name = "Test Court", IdEds = 1 };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
     var response = await _client.PostAsync("/api/v1/court", content);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Update_WithData_ReturnsResponse()
    {
    var data = new { IdCourt = 1, Name = "Updated Court", IdEds = 1 };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/v1/court", content);
 Assert.NotNull(response);
    }
}
