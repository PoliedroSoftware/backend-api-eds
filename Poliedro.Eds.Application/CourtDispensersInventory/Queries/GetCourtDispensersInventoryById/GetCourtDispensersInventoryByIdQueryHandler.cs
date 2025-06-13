using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GetCourtDispensersInventoryById
{
    public class GetCourtDispensersInventoryByIdQueryHandler(
        ICourtDispensersInventoryGetByIdCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper)
        : IRequestHandler<GetCourtDispensersInventoryByIdQuery, Result<CourtDispensersInventoryDto, Error>>
    {
        public async Task<Result<CourtDispensersInventoryDto, Error>> Handle(GetCourtDispensersInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CourtDispensersInventoryDto>(result.Value);
        }
    }
}
