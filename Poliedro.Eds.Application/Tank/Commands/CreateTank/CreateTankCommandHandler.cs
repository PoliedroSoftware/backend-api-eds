using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public class CreateTankCommandHandler(
    ITankCreateTank tankDomainTank,
    IMapper mapper) : IRequestHandler<CreateTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateTankCommand request, CancellationToken cancellationToken)
    {      

        var tankEntity = mapper.Map<TankEntity>(request.Request);
        var result = await tankDomainTank.CreateAsync(tankEntity);
        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;    
    }
}



