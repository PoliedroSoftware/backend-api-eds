using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Domain.Eds.DomainEds;

namespace Poliedro.Eds.Application.Eds.Queries.GellAllEds;
public class GetAllEdsQueryHandler
(
    IEdsGetAllService EdsGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllEdsQuery, IEnumerable<EdsDto>>
{
    public async Task<IEnumerable<EdsDto>> Handle(GellAllEdsQuery request, CancellationToken cancellationToken)
    {
        var result = await EdsGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<EdsDto>>(result);
    }
}