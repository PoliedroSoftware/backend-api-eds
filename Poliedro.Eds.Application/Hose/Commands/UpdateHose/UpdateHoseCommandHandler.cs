using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;

namespace Poliedro.Eds.Application.Hose.Commands.UpdateHose
{
    public class UpdateHoseCommandHandler(
        IHoseUpdateHose hoseDomainHose,
        IMapper mapper
    ) : IRequestHandler<UpdateHoseCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateHoseCommand request, CancellationToken cancellationToken)
        {
            var hoseEntity = mapper.Map<HoseEntity>(request);
            var result = await hoseDomainHose.UpdateAsync(hoseEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}