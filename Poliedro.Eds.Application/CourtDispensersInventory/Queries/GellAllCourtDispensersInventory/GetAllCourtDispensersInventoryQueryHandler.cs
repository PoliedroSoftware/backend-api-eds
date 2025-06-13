using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GellAllCourtDispensersInventory;
public class GetAllCourtDispensersInventoryQueryHandler
(
    ICourtDispensersInventoryGetAllCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
    IMapper mapper)
    : IRequestHandler<GellAllCourtDispensersInventoryQuery, IEnumerable<CourtDispensersInventoryDto>>
{
    public async Task<IEnumerable<CourtDispensersInventoryDto>> Handle(GellAllCourtDispensersInventoryQuery request, CancellationToken cancellationToken)
    {
        var result = await courtdispensersinventoryDomainCourtDispensersInventory.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<CourtDispensersInventoryDto>>(result);
    }
}

