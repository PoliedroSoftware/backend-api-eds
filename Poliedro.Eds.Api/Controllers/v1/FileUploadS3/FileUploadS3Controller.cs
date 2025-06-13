using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.FileUploadS3.Command;
using Poliedro.Eds.Domain.FileUploadS3;

namespace Adapters.Controllers;

[ApiController]
[Route("api/v1/files")]
public class UploadController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        var results = new List<string>();
        foreach (var file in request.Files)
        {
            var command = new UploadFileCommand(file);
            var result = await mediator.Send(command);
            results.Add(result);
        }
        return Ok(new { Urls = results });
    }
}