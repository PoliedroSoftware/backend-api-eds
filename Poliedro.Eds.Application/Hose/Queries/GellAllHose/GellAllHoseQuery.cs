using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GellAllHose;

public record GellAllHoseQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<HoseDto>>;
