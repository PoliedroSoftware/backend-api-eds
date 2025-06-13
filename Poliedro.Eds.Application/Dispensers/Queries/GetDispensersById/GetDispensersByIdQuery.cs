using MediatR;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Dispensers.Queries.GetDispensersById;
public record GetDispensersByIdQuery (int Id) : IRequest<Result<DispensersDto, Error>>;
