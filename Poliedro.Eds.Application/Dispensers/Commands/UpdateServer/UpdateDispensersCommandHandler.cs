using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.Commands.UpdateDispensers
{
    public class UpdateDispensersCommandHandler(
        IDispensersUpdateDispensers dispensersDomainDispensers,
        IMapper mapper
   ) : IRequestHandler<UpdateDispensersCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateDispensersCommand request, CancellationToken cancellationToken)
        {
            var dispensersEntity = mapper.Map<DispensersEntity>(request);
            var result = await dispensersDomainDispensers.UpdateAsync(dispensersEntity);

            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
