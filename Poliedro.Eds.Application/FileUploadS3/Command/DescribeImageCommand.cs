using MediatR;
using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public record DescribeImageCommand(IFormFile Image) : IRequest<string>;