using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtList;

public class GetCourtsListQueryHandler(
        ICourtListDomainService courtListDomainService,
        IMapper mapper) : IRequestHandler<GetCourtsListQuery, IEnumerable<CourtListResponseDto>>
{
    public async Task<IEnumerable<CourtListResponseDto>> Handle(GetCourtsListQuery request, CancellationToken cancellationToken)
    => await courtListDomainService.GetAllAsync(request.PaginationParams);
}

