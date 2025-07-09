using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

public class FileStorageService(IAmazonS3 amazonS3) : IFileStorageService
{
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName)
    {
        var uploadRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = "application/octet-stream"
        };

        var response = await amazonS3.PutObjectAsync(uploadRequest);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return $"https://{bucketName}.s3.amazonaws.com/{fileName}";
        }
        else
        {
            throw new Exception("Error uploading file to S3.");
        }
    }
}
