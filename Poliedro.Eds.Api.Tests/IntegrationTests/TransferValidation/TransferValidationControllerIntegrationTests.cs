using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.TransferValidation;

public class TransferValidationControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public TransferValidationControllerIntegrationTests(CustomWebApplicationFactory factory) => 
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

 [Fact]
 public async Task GetAll_ReturnsResponse() => 
  Assert.NotNull(await _client.GetAsync("/api/v1/transfervalidation?PageNumber=1&PageSize=10"));

    [Fact]
    public async Task GetById_ReturnsResponse() => 
        Assert.NotNull(await _client.GetAsync("/api/v1/transfervalidation/1"));

    [Fact]
    public async Task Create_ReturnsResponse()
    {
   var content = new StringContent(JsonSerializer.Serialize(new { TransferCode = "TEST123", Amount = 1000 }), Encoding.UTF8, "application/json");
        Assert.NotNull(await _client.PostAsync("/api/v1/transfervalidation", content));
    }
}
