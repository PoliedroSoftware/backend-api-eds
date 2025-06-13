using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.Commands.UpdateTank;

public class UpdateTankCommandHandler(
    ITankUpdateTank tankDomainTank,
    IMapper mapper
) : IRequestHandler<UpdateTankCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateTankCommand request, CancellationToken cancellationToken)
    {
        var tankEntity = mapper.Map<TankEntity>(request);
        var result = await tankDomainTank.UpdateAsync(tankEntity);

        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}