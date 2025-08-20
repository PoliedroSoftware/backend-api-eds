using MediatR;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Application.FileUploadS3.Command;

public class DescribeImageHandler(IImageDescriptionService imageDescriptionService) : IRequestHandler<DescribeImageCommand, string>
{
    public async Task<string> Handle(DescribeImageCommand request, CancellationToken cancellationToken) => 
        await imageDescriptionService.DescribeImageAsync(request.Image);
}