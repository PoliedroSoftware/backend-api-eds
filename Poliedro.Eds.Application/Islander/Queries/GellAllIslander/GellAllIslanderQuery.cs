using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Islander.Queries.GellAllIslander;

public record GellAllIslanderQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<IslanderDto>>;
