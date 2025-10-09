using MediatR;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Dispensers.Queries.GellAllDispensers;

public record GellAllDispensersQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<DispensersDto>>;
