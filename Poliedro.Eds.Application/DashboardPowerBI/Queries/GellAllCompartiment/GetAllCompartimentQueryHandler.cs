using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCompartiment;
public class GetAllCompartimentQueryHandler
(
    ICompartimentGetAllCompartiment compartimentDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllCompartimentQuery, IEnumerable<CompartimentDto>>
{
    public async Task<IEnumerable<CompartimentDto>> Handle(GellAllCompartimentQuery request, CancellationToken cancellationToken)
    {
        var result = await compartimentDomainService.GetAllAsync(request.PaginationParams);
        var ccc = mapper.Map<List<CompartimentDto>>(result);
        return ccc;
    }
}


