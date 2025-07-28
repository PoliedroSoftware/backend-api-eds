using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using System.Net;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory
{
    public class CreateCourtDispensersInventoryCommandHandler(
        ICourtDispensersInventoryCreateCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper,
        IValidator<CreateCourtDispensersInventoryRequestDto> validator
        ) : IRequestHandler<CreateCourtDispensersInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCourtDispensersInventoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var courtdispensersinventoryEntity = mapper.Map<CourtDispensersInventoryEntity>(request.Request);
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.CreateAsync(courtdispensersinventoryEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}



