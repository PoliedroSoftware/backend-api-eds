using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Domain.FileUploadS3.Ports;
using System.Net.Http.Headers;

namespace Amazon.S3.FileUploadService;

public class FileUploadService(IConfiguration configuration) : IFileUploadService
{
    private readonly string _uploadUrl = configuration["AWS:UploadEndpoint"];

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        using var httpClient = new HttpClient();
        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
       
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        content.Add(fileContent, "files", file.FileName);

        var response = await httpClient.PostAsync(_uploadUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error al subir el archivo: {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsStringAsync();
    }
}
