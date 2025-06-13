using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtById;

public record GetCourtByIdQuery(int Id) : IRequest<Result<CourtDto, Error>>;

