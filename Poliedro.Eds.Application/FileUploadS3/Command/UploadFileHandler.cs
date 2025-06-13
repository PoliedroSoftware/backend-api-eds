using MediatR;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public class UploadFileHandler(IFileUploadService fileUploadService) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken) => await fileUploadService.UploadFileAsync(request.File);
}