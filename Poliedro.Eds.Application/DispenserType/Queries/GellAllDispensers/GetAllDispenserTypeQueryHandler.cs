using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;

namespace Poliedro.Eds.Application.DispenserType.Queries.GellAllDispenserType;
public class GetAllDispenserTypeQueryHandler
(
    IDispenserTypeGetAllDispenserType dispenserTypeDomainService,
    IMapper mapper)
    : IRequestHandler<GellAllDispenserTypeQuery, IEnumerable<DispenserTypeDto>>
{
    public async Task<IEnumerable<DispenserTypeDto>> Handle(GellAllDispenserTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await dispenserTypeDomainService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<DispenserTypeDto>>(result);
    }
}


