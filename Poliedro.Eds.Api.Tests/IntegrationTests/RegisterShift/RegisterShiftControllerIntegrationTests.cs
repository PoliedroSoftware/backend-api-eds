using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.RegisterShift;

public class RegisterShiftControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterShiftControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
      _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task GetById_WithId_ReturnsResponse()
    {
        var response = await _client.GetAsync("/api/v1/registershift/1");
        Assert.NotNull(response);
    }

    [Fact]
 public async Task Create_WithData_ReturnsResponse()
    {
        var data = new { IdIslander = 1, ShiftStart = DateTime.Now, IdEds = 1 };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
      var response = await _client.PostAsync("/api/v1/registershift", content);
        Assert.NotNull(response);
  }
}
