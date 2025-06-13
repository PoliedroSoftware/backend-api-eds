using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Domain.FileUploadS3.Ports;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file);
}
