using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetCourtList;

public class GetCourtsListQueryHandler(
        ICourtListDomainService courtListDomainService,
        IMapper mapper) : IRequestHandler<GetCourtsListQuery, IEnumerable<Dtos.Court.CourtListResponseDto>>
{
    public async Task<IEnumerable<Dtos.Court.CourtListResponseDto>> Handle(GetCourtsListQuery request, CancellationToken cancellationToken)
    {
        var result = await courtListDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<IEnumerable<Dtos.Court.CourtListResponseDto>>(result);
    }

}

