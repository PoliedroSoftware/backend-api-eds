using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllEds;

public record GellAllEdsQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<EdsDto>>;