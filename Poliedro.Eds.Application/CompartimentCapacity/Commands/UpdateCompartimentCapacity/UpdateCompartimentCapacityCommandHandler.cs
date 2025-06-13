using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;

    public class UpdateCompartimentCapacityCommandHandler(
        ICompartimentCapacityUpdateCompartimentCapacity CompartimentCapacityDomainCompartimentCapacity,
        IMapper mapper
    ) : IRequestHandler<UpdateCompartimentCapacityCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCompartimentCapacityCommand request, CancellationToken cancellationToken)
        {
            var CompartimentCapacityEntity = mapper.Map<CompartimentCapacityEntity>(request);
            var result = await CompartimentCapacityDomainCompartimentCapacity.UpdateAsync(CompartimentCapacityEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }