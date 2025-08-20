using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory
{
    public class UpdateCourtDispensersInventoryCommandHandler(
        ICourtDispensersInventoryUpdateCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper,
        IValidator<UpdateCourtDispensersInventoryCommand> validator
        ) : IRequestHandler<UpdateCourtDispensersInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCourtDispensersInventoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var courtdispensersinventoryEntity = mapper.Map<CourtDispensersInventoryEntity>(request);
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.UpdateAsync(courtdispensersinventoryEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}
