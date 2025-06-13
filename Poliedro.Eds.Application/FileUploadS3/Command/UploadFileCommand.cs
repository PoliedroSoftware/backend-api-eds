using MediatR;
using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public record UploadFileCommand(IFormFile File) : IRequest<string>;
