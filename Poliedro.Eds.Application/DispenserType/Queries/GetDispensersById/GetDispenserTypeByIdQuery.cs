using MediatR;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.DispenserType.Queries.GetDispenserTypeById;

public record GetDispenserTypeByIdQuery (int Id): IRequest<Result<DispenserTypeDto, Error>>;
