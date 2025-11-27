using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.FileUploadS3;
using Poliedro.Eds.Domain.FileUploadS3.Ports;
using RabbitMQ.Client;

namespace Amazon.S3.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _folderName;
        private readonly string _hostName;
        private readonly string _queue;
        private readonly string _region;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IConfiguration configuration, ILogger<FileUploadService> logger)
        {
            _logger = logger;
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName configuration is missing");
            _region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");
            _folderName = configuration["AWS:FolderName"] ?? "carpeta";

            var regionEndpoint = RegionEndpoint.GetBySystemName(_region);
            _s3Client = new AmazonS3Client(regionEndpoint);

            //RabbitMQ config
            _hostName = configuration["RabbitMQ:HostName"] ?? throw new ArgumentNullException("HostName configuration is missing");
            _queue = configuration["RabbitMQ:QueueDocuments"] ?? throw new ArgumentNullException("Queue configuration is missing");
        }

        public async Task<string> UploadFileAsync(IFormFile file, int courtId)
        {
            // Siempre agregar prefijo del courtId al nombre del archivo
            var fileName = $"{courtId}_{file.FileName}";
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{fileName}");
            
            await using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var message = new DocumentEvent
            {
                BucketName = _bucketName,
                FolderName = _folderName,
                TempPath = tempPath,
                FileName = fileName,
                ContentType = file.ContentType,
                CourtId = courtId
            };

            try
            {
                // Publish RabbitMQ
                var factory = new ConnectionFactory() 
                { 
                    HostName = _hostName,
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(5),
                    AutomaticRecoveryEnabled = false
                };
                
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queue, durable: true, exclusive: false, autoDelete: false);
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: string.Empty, routingKey: _queue, basicProperties: null, body: body);
                
                _logger.LogInformation("File upload message sent to RabbitMQ queue for file: {FileName}", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send file upload message to RabbitMQ. File will remain in temp location: {TempPath}", tempPath);
                // No lanzar excepción, permitir que el proceso continúe
            }

            return $"{_folderName}/pending/{fileName}";
        }

        public async Task<List<string>> GetCourtImagesAsync(int courtId)
        {
            var images = new List<string>();
            var prefix = $"{_folderName}/{courtId}_";

            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = prefix
                };

                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);

                    foreach (var s3Object in response.S3Objects)
                    {
                        var url = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{s3Object.Key}";
                        images.Add(url);
                    }

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated == true);

                _logger.LogInformation("Retrieved {Count} images for court {CourtId}", images.Count, courtId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving images for court {CourtId}", courtId);
                throw;
            }

            return images;
        }
    }
}
