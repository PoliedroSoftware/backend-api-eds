using MediatR;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Island.Queries.GellAllIsland;

public record GellAllIslandQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<IslandDto>>;
