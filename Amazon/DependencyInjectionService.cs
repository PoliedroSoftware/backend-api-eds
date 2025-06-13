using Amazon.S3.FileUploadService;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Infraestructure.External.Plemsi;

public static class DependencyInjectionService
{
    public static IServiceCollection AddExternalAmazon(this IServiceCollection services)
    {
        services.AddTransient<IFileUploadService, FileUploadService>();
        return services;
    }
}
