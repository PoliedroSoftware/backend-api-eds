using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.CreateCompartimentCapacity;
    public class CreateCompartimentCapacityCommandHandler(
        ICompartimentCapacityCreateCompartimentCapacity CompartimentCapacityDomainCompartimentCapacity,
        IMapper mapper) : IRequestHandler<CreateCompartimentCapacityCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCompartimentCapacityCommand request, CancellationToken cancellationToken)
        {
            var CompartimentCapacityEntity = mapper.Map<CompartimentCapacityEntity>(request.Request);
            var result = await CompartimentCapacityDomainCompartimentCapacity.CreateAsync(CompartimentCapacityEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }