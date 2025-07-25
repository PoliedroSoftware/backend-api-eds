using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Tank.Commands.UpdateTank;

public class UpdateTankCommandHandler(
    ITankUpdateTank tankDomainTank,
    IMapper mapper,
    IValidator<UpdateTankCommand> validator
) : IRequestHandler<UpdateTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateTankCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var tankEntity = mapper.Map<TankEntity>(request);
        var result = await tankDomainTank.UpdateAsync(tankEntity);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}