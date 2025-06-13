using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Business.DomainBusiness;

namespace Poliedro.Eds.Application.Business.Queries.GellAllBusiness;
public class GetAllBusinessQueryHandler
(
    IBusinessGetAllService BusinessGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllBusinessQuery, IEnumerable<BusinessDto>>
{
    public async Task<IEnumerable<BusinessDto>> Handle(GellAllBusinessQuery request, CancellationToken cancellationToken)
    {
        var result = await BusinessGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<BusinessDto>>(result);
    }
}