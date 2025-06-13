using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Application.Capacity.Commands.CreateCapacity;

public class CreateCapacityCommandHandler(
    ICapacityCreateService CapacityCreateService,
    IMapper mapper) : IRequestHandler<CreateCapacityCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateCapacityCommand request, CancellationToken cancellationToken)
    {
        var CapacityEntity = mapper.Map<CapacityEntity>(request.Request);
        var result = await CapacityCreateService.CreateAsync(CapacityEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}