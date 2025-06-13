using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.UpdateCompartiment
{
    public class UpdateCompartimentCommandHandler(
        ICompartimentUpdateCompartiment compartimentDomainCompartiment,
        IMapper mapper
   ) : IRequestHandler<UpdateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCompartimentCommand request, CancellationToken cancellationToken)
        {
            var compartimentEntity = mapper.Map<CompartimentEntity>(request);
            var result = await compartimentDomainCompartiment.UpdateAsync(compartimentEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
