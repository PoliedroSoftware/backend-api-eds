using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
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
        IValidator<CreateCourtDispensersInventoryRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateCourtDispensersInventoryCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCourtDispensersInventoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await courtdispensersinventoryDomainCourtDispensersInventory.CreateAsync(mapper.Map<CourtDispensersInventoryEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COURT_DISPENSERS_INVENTORY);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}



