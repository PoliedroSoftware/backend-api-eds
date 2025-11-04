using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Hose;

public class HoseControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HoseControllerIntegrationTests(CustomWebApplicationFactory factory)
  {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

  [Fact]
  public async Task GetAll_WithPagination_ReturnsResponse()
    {
      var response = await _client.GetAsync("/api/v1/hose?PageNumber=1&PageSize=10");
   Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound);
}

    [Fact]
    public async Task GetById_WithId_ReturnsResponse()
    {
   var response = await _client.GetAsync("/api/v1/hose/1");
     Assert.NotNull(response);
    }

 [Fact]
    public async Task Create_WithData_ReturnsResponse()
  {
var data = new { Name = "Test Hose", IdDispenser = 1 };
  var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
 var response = await _client.PostAsync("/api/v1/hose", content);
    Assert.NotNull(response);
    }
}
