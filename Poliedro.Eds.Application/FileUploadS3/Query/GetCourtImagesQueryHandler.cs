using System.Net;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Application.FileUploadS3.Query;

public class GetCourtImagesQueryHandler(IFileUploadService fileUploadService)
    : IRequestHandler<GetCourtImagesQuery, Result<List<string>, Error>>
{
    public async Task<Result<List<string>, Error>> Handle(GetCourtImagesQuery request, CancellationToken cancellationToken)
    {
        if (request.CourtId <= 0)
        {
            return Result<List<string>, Error>.Failure(
                Error.CreateInstance("InvalidCourtId", "Court ID must be greater than 0", HttpStatusCode.BadRequest));
        }

        return await fileUploadService.GetCourtImagesAsync(request.CourtId);
    }
}
