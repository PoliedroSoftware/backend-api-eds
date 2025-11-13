namespace Poliedro.Eds.Domain.FileUploadS3.Ports;

public interface IS3UrlGenerator
{
    /// <summary>
    /// Genera una URL pre-firmada para acceder a un archivo en S3
    /// </summary>
    /// <param name="s3Key">La ruta completa del archivo en S3</param>
    /// <param name="expirationMinutes">Tiempo de expiración de la URL en minutos (por defecto 60)</param>
    /// <returns>URL pre-firmada para acceder al archivo</returns>
    Task<string> GeneratePresignedUrlAsync(string s3Key, int expirationMinutes = 60);
}
