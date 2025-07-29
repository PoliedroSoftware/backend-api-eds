using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Amazon.S3.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _folderName;

        public FileUploadService(IConfiguration configuration)
        {
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName configuration is missing");
            var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("Region configuration is missing");
            _folderName = configuration["AWS:FolderName"] ?? "carpeta";

            var regionEndpoint = RegionEndpoint.GetBySystemName(region);
            _s3Client = new AmazonS3Client(regionEndpoint);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileKey = $"{_folderName}/{Guid.NewGuid()}_{file.FileName}";

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var uploadRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey,
                    InputStream = memoryStream,
                    ContentType = file.ContentType
                };

                var response = await _s3Client.PutObjectAsync(uploadRequest);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Error al subir el archivo: {response.HttpStatusCode}");
                }

                return fileKey;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir el archivo a S3: {ex.Message}", ex);
            }
        }
    }
}
