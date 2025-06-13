using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Domain.FileUploadS3;

public class UploadFileRequest
{
    public List<IFormFile> Files { get; set; }
}
