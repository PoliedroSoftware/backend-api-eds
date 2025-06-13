using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory
{
    public class CreateCourtDispensersInventoryCommandHandler(
        ICourtDispensersInventoryCreateCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper) : IRequestHandler<CreateCourtDispensersInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCourtDispensersInventoryCommand request, CancellationToken cancellationToken)
        {      

            var courtdispensersinventoryEntity = mapper.Map<CourtDispensersInventoryEntity>(request.Request);
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.CreateAsync(courtdispensersinventoryEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;    
        }
    }
}



