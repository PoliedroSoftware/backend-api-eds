using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;

namespace Poliedro.Eds.Application.Dispensers.Queries.GellAllDispensers;
public class GetAllDispensersQueryHandler
(
    IDispensersGetAllDispensers dispensersDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllDispensersQuery, IEnumerable<DispensersDto>>
{
    public async Task<IEnumerable<DispensersDto>> Handle(GellAllDispensersQuery request, CancellationToken cancellationToken)
    {
        var result = await dispensersDomainService.GetAllAsync(request.PaginationParams);
        var ccc = mapper.Map<List<DispensersDto>>(result);
        return ccc;
    }
}


