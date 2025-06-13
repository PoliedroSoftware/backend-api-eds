using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory
{
    public class UpdateCourtDispensersInventoryCommandHandler(
        ICourtDispensersInventoryUpdateCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper
    ) : IRequestHandler<UpdateCourtDispensersInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCourtDispensersInventoryCommand request, CancellationToken cancellationToken)
        {
            var courtdispensersinventoryEntity = mapper.Map<CourtDispensersInventoryEntity>(request);
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.UpdateAsync(courtdispensersinventoryEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}