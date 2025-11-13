using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Amazon.S3.FileUploadService;

public class S3UrlGeneratorService : IS3UrlGenerator
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly ILogger<S3UrlGeneratorService> _logger;

    public S3UrlGeneratorService(IConfiguration configuration, ILogger<S3UrlGeneratorService> logger)
    {
        _logger = logger;
        _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName configuration is missing");
        var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");

        var regionEndpoint = RegionEndpoint.GetBySystemName(region);
        _s3Client = new AmazonS3Client(regionEndpoint);
    }

    public async Task<string> GeneratePresignedUrlAsync(string s3Key, int expirationMinutes = 60)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(s3Key))
            {
                _logger.LogWarning("S3 key is null or empty, returning empty URL");
                return string.Empty;
            }

            // Verificar si el archivo existe antes de generar la URL
            try
            {
                var metadataRequest = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = s3Key
                };
                await _s3Client.GetObjectMetadataAsync(metadataRequest);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("File not found in S3: {S3Key}", s3Key);
                return string.Empty;
            }

            // Generar URL pre-firmada
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = s3Key,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Protocol = Protocol.HTTPS
            };

            var url = _s3Client.GetPreSignedURL(request);
            _logger.LogInformation("Pre-signed URL generated for: {S3Key}, expires in {Minutes} minutes", s3Key, expirationMinutes);
            
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating pre-signed URL for S3 key: {S3Key}", s3Key);
            return string.Empty;
        }
    }
}
