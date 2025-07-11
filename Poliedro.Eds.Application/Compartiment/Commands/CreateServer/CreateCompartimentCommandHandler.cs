using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment
{
    public class CreateCompartimentCommandHandler(
        ICompartimentCreateCompartiment compartimentDomainService,
        IMapper mapper) : IRequestHandler<CreateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCompartimentCommand request, CancellationToken cancellationToken)
        {
            var compartimentEntity = mapper.Map<CompartimentEntity>(request.Request);
            var result = await compartimentDomainService.CreateAsync(compartimentEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}




