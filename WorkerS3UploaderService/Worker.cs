using System.Text;
using System.Text.Json;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.FileUploadS3;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Domain.Islander.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkerS3UploaderService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;
        private readonly string _bucketName;
        private readonly string _folderName;
        private readonly IAmazonS3 _s3Client;
        private readonly string _hostName;
        private readonly string _queue;
        private readonly IConnection _rabbitConnection;

        public Worker(IConnection rabbitConnection, ILogger<Worker> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitConnection = rabbitConnection;
            _logger = logger;
            //RabbitMQ config
            _queue = configuration["RabbitMQ:QueueDocuments"] ?? throw new ArgumentNullException("Queue configuration is missing");

            //S3 config
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName configuration is missing");
            var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");
            _folderName = configuration["AWS:FolderName"] ?? "carpeta";

            var regionEndpoint = RegionEndpoint.GetBySystemName(region);
            _s3Client = new AmazonS3Client(regionEndpoint);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_rabbitConnection == null)
            {
                _logger.LogWarning("RabbitMQ connection is not available. Worker will not process documents.");
                return;
            }

            if (!_rabbitConnection.IsOpen)
            {
                _logger.LogWarning("RabbitMQ connection is not open. Worker will not process documents.");
                return;
            }

            try
            {
                var channel = _rabbitConnection.CreateModel();

                channel.QueueDeclare(queue: _queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _logger.LogInformation($"Escuchando la cola '{_queue}' cada 5 segundos...");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = channel.BasicGet(queue: _queue, autoAck: false);

                    if (result != null)
                    {
                        var json = Encoding.UTF8.GetString(result.Body.ToArray());
                        var doc = JsonSerializer.Deserialize<DocumentEvent>(json);
                        _logger.LogInformation($"Documento recibido: {json}");

                        try
                        {
                            var key = $"{doc.FolderName}/{Guid.NewGuid()}_{doc.FileName}";
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                BucketName = doc.BucketName,
                                FilePath = doc.TempPath,
                                Key = key,
                                ContentType = doc.ContentType
                            };

                            var transferUtility = new TransferUtility(_s3Client);
                            await transferUtility.UploadAsync(uploadRequest, stoppingToken);

                            _logger.LogInformation($"Subido a S3: {key}");

                            if (File.Exists(doc.TempPath))
                            {
                                File.Delete(doc.TempPath);
                            }

                            channel.BasicAck(result.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error subiendo {doc?.FileName} Error: {ex.Message}");
                            channel.BasicNack(result.DeliveryTag, false, true); // retry
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No hay documentos en la cola.");
                    }
                    var delay = _configuration.GetValue<int>("worker:PollingInterval", 30000);
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in S3 Uploader Worker execution. RabbitMQ may be unavailable.");
            }
        }
    }
}
