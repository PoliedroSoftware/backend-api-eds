using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetCourtList;

public record GetCourtsListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<Dtos.Court.CourtListResponseDto>>;
