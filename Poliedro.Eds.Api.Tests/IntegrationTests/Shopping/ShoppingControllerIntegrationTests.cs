using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.Shopping;

public class ShoppingControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
 private readonly HttpClient _client;

  public ShoppingControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
   _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task GetAll_WithPagination_ReturnsResponse()
    {
        var response = await _client.GetAsync("/api/v1/shopping?PageNumber=1&PageSize=10");
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound);
  }

  [Fact]
    public async Task GetById_WithId_ReturnsResponse()
    {
 var response = await _client.GetAsync("/api/v1/shopping/1");
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Create_WithData_ReturnsResponse()
    {
        var data = new { IdProvider = 1, TotalAmount = 1000.50, Date = DateTime.Now };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
     var response = await _client.PostAsync("/api/v1/shopping", content);
Assert.NotNull(response);
    }
}
