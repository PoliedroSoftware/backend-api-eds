using Amazon.IoT.PublishService;
using Amazon.S3.FileUploadService;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Domain.FileUploadS3.Ports;
using Poliedro.Eds.Domain.IoT.Ports;

namespace Poliedro.Eds.Infraestructure.External.Plemsi;

public static class DependencyInjectionService
{
    public static IServiceCollection AddExternalAmazon(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IFileUploadService, FileUploadService>();
        services.AddTransient<IS3UrlGenerator, S3UrlGeneratorService>();
        services.AddTransient<IIoTPublishService, IoTPublishService>();
        return services;
    }
}
