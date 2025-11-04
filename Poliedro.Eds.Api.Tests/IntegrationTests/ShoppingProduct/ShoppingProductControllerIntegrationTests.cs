using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.ShoppingProduct;

public class ShoppingProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
  public ShoppingProductControllerIntegrationTests(CustomWebApplicationFactory factory) => 
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

[Fact]
  public async Task GetAll_ReturnsResponse() => 
        Assert.NotNull(await _client.GetAsync("/api/v1/shoppingproduct?PageNumber=1&PageSize=10"));

    [Fact]
    public async Task GetById_ReturnsResponse() => 
  Assert.NotNull(await _client.GetAsync("/api/v1/shoppingproduct/1"));

    [Fact]
    public async Task Create_ReturnsResponse()
    {
        var content = new StringContent(JsonSerializer.Serialize(new { IdShopping = 1, IdProduct = 1, Quantity = 10 }), Encoding.UTF8, "application/json");
     Assert.NotNull(await _client.PostAsync("/api/v1/shoppingproduct", content));
    }
}
