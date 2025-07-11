using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GellAllCompartimentCapacity;
public class GetAllCompartimentCapacityQueryHandler
(
    ICompartimentCapacityGetAllCompartimentCapacity CompartimentCapacityDomainCompartimentCapacity,
    IMapper mapper)
    : IRequestHandler<GellAllCompartimentCapacityQuery, IEnumerable<CompartimentCapacityDto>>
{
    public async Task<IEnumerable<CompartimentCapacityDto>> Handle(GellAllCompartimentCapacityQuery request, CancellationToken cancellationToken)
    {
        var result = await CompartimentCapacityDomainCompartimentCapacity.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<CompartimentCapacityDto>>(result);
    }
}