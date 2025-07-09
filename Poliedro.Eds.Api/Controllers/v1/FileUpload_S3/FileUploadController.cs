using Microsoft.AspNetCore.Mvc;

[Route("api/v1/FileUpload_S3")]
[ApiController]
public class FileUploadController(IFileStorageService fileStorageService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var fileUrl = await fileStorageService.UploadFileAsync(stream, file.FileName, "your-bucket-name");
            return Ok(new { FileUrl = fileUrl });
        }
        return BadRequest("No file uploaded.");
    }
}