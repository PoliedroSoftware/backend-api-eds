using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Poliedro.Eds.Api.Tests.Infrastructure;

namespace Poliedro.Eds.Api.Tests.IntegrationTests.FileUploadS3;

public class FileUploadS3ControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;
    public FileUploadS3ControllerIntegrationTests(CustomWebApplicationFactory factory) => 
   _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task UploadFile_ReturnsResponse()
    {
 var content = new MultipartFormDataContent();
    var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
   fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
      content.Add(fileContent, "file", "test.txt");
   Assert.NotNull(await _client.PostAsync("/api/v1/fileupload/upload", content));
 }

    [Fact]
    public async Task GetFiles_ReturnsResponse() => 
        Assert.NotNull(await _client.GetAsync("/api/v1/fileupload/list"));
}
