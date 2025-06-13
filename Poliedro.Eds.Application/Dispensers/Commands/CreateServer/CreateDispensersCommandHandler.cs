using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.Commands.CreateDispensers
{
    public class CreateDispensersCommandHandler(
        IDispensersCreateDispensers dispensersDomainService,
        IMapper mapper) : IRequestHandler<CreateDispensersCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispensersCommand request, CancellationToken cancellationToken)
        {
            var dispensersEntity = mapper.Map<DispensersEntity>(request.Request);
            var result = await dispensersDomainService.CreateAsync(dispensersEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}




