using MediatR;
using Poliedro.Eds.Application.Court.Dtos.View;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtList;

public record GetCourtsListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CourtListResponseDto>>;
