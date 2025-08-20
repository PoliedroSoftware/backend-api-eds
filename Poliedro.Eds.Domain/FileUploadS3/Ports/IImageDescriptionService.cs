using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Domain.FileUploadS3.Ports;

public interface IImageDescriptionService
{
    Task<string> DescribeImageAsync(IFormFile image);
    Task<string> DescribeImageAsync(string imageUrl);
}