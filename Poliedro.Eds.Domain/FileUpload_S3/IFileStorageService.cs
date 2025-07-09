public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName);
}
