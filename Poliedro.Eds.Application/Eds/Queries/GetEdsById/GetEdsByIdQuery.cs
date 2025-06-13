using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Eds.Queries.GetEdsById;

public record GetEdsByIdQuery (int Id) : IRequest<Result<EdsDto, Error>>;