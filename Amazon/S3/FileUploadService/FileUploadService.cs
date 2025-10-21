using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public FileUploadService(IConfiguration configuration)
        {
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName configuration is missing");
            var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");
            _folderName = configuration["AWS:FolderName"] ?? "carpeta";

            var regionEndpoint = RegionEndpoint.GetBySystemName(region);
            _s3Client = new AmazonS3Client(regionEndpoint);

            //RabbitMQ config
            _hostName = configuration["RabbitMQ:HostName"] ?? throw new ArgumentNullException("HostName configuration is missing");
            _queue = configuration["RabbitMQ:QueueDocuments"] ?? throw new ArgumentNullException("Queue configuration is missing");
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{file.FileName}");
            await using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var message = new DocumentEvent
            {
                BucketName = _bucketName,
                FolderName = _folderName,
                TempPath = tempPath,
                FileName = file.FileName,
                ContentType = file.ContentType
            };

            // Publish RabbitMQ
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queue, durable: true, exclusive: false, autoDelete: false);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "documents", basicProperties: null, body: body);

            return $"{_folderName}/pending/{file.FileName}";
        }
    }
}
