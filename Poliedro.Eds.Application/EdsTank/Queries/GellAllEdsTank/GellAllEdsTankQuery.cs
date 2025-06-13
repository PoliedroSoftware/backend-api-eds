using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.EdsTank.Queries.GellAllEdsTank;

public record GellAllEdsTankQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<EdsTankDto>>;
