using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;

namespace Poliedro.Eds.Application.Capacity.Queries.GellAllCapacity;
public class GetAllCapacityQueryHandler
(
    ICapacityGetAllService CapacityGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllCapacityQuery, IEnumerable<CapacityDto>>
{
    public async Task<IEnumerable<CapacityDto>> Handle(GellAllCapacityQuery request, CancellationToken cancellationToken)
    {
        var result = await CapacityGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<CapacityDto>>(result);
    }
}