using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.WhatsApp;

public class WhatsAppControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public WhatsAppControllerIntegrationTests(CustomWebApplicationFactory factory) => 
   _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task SendMessage_ReturnsResponse()
    {
        var content = new StringContent(JsonSerializer.Serialize(new { PhoneNumber = "1234567890", Message = "Test" }), Encoding.UTF8, "application/json");
        Assert.NotNull(await _client.PostAsync("/api/v1/whatsapp/send", content));
 }

    [Fact]
    public async Task GetStatus_ReturnsResponse() => 
   Assert.NotNull(await _client.GetAsync("/api/v1/whatsapp/status"));
}
