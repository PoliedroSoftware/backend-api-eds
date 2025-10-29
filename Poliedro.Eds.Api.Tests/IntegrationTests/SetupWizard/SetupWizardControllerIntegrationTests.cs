using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.SetupWizard;

public class SetupWizardControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public SetupWizardControllerIntegrationTests(CustomWebApplicationFactory factory) => 
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

  [Fact]
  public async Task GetWizardStatus_ReturnsResponse() => 
        Assert.NotNull(await _client.GetAsync("/api/v1/setupwizard/status"));

    [Fact]
  public async Task CompleteStep_ReturnsResponse()
    {
   var content = new StringContent(JsonSerializer.Serialize(new { Step = 1, Completed = true }), Encoding.UTF8, "application/json");
   Assert.NotNull(await _client.PostAsync("/api/v1/setupwizard/complete", content));
    }
}
