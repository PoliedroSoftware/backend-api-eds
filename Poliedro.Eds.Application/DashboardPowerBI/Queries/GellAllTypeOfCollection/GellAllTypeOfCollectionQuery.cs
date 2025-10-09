using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllTypeOfCollection;

public record GellAllTypeOfCollectionQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<TypeOfCollectionDto>>;
