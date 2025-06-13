using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Eds.Queries.GellAllEds;

public record GellAllEdsQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<EdsDto>>;