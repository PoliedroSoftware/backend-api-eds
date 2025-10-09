using MediatR;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.DispenserType.Queries.GellAllDispenserType;

public record GellAllDispenserTypeQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<DispenserTypeDto>>;
