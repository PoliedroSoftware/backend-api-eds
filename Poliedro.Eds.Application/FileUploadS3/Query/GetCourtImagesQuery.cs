using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.FileUploadS3.Query;

public record GetCourtImagesQuery(int CourtId) : IRequest<Result<List<string>, Error>>;
