using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.OpenAI;

public class OpenAIControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public OpenAIControllerIntegrationTests(CustomWebApplicationFactory factory) => 
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

  [Fact]
    public async Task GenerateText_ReturnsResponse()
  {
        var content = new StringContent(JsonSerializer.Serialize(new { Prompt = "Test prompt" }), Encoding.UTF8, "application/json");
   Assert.NotNull(await _client.PostAsync("/api/v1/openai/generate", content));
    }

    [Fact]
    public async Task GetModels_ReturnsResponse() => 
        Assert.NotNull(await _client.GetAsync("/api/v1/openai/models"));
}
