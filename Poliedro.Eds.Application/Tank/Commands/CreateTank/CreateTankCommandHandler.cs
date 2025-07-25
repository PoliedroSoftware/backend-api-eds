using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;
using System.Net;

namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public class CreateTankCommandHandler(
    ITankCreateTank tankDomainTank,
    IMapper mapper,
    IValidator<CreateTankRequestDto> validator
    ) : IRequestHandler<CreateTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateTankCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var tankEntity = mapper.Map<TankEntity>(request.Request);
        var result = await tankDomainTank.CreateAsync(tankEntity);
        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;    
    }
}



