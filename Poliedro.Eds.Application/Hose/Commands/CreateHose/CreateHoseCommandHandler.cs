using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Application.Hose.Commands.CreateHose
{
    public class CreateHoseCommandHandler(
        IHoseCreateHose hoseDomainHose,
        IMapper mapper) : IRequestHandler<CreateHoseCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateHoseCommand request, CancellationToken cancellationToken)
        {      

            var hoseEntity = mapper.Map<HoseEntity>(request.Request);
            var result = await hoseDomainHose.CreateAsync(hoseEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;    
        }
    }
}



