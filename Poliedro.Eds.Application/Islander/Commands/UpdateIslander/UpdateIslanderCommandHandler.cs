using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Application.Islander.Commands.UpdateIslander
{
    public class UpdateIslanderCommandHandler(
        IIslanderUpdateIslander islanderDomainIslander,
        IMapper mapper
    ) : IRequestHandler<UpdateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateIslanderCommand request, CancellationToken cancellationToken)
        {
            var islanderEntity = mapper.Map<IslanderEntity>(request);
            var result = await islanderDomainIslander.UpdateAsync(islanderEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}