
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllBusiness;

    public record GellAllBusinessQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<Business2Dto>>;


