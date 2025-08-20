using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GetCourtDispensersInventoryById
{
    public class GetCourtDispensersInventoryByIdQueryHandler(
        ICourtDispensersInventoryGetByIdCourtDispensersInventory courtdispensersinventoryDomainCourtDispensersInventory,
        IMapper mapper,
        IValidator<GetCourtDispensersInventoryByIdQuery> validator)
        : IRequestHandler<GetCourtDispensersInventoryByIdQuery, Result<CourtDispensersInventoryDto, Error>>
    {
        public async Task<Result<CourtDispensersInventoryDto, Error>> Handle(GetCourtDispensersInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<CourtDispensersInventoryDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await courtdispensersinventoryDomainCourtDispensersInventory.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CourtDispensersInventoryDto>(result.Value);
        }
    }
}
