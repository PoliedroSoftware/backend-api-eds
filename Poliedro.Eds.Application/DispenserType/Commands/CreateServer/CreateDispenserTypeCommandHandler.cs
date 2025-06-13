using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType

{
    public class CreateDispenserTypeCommandHandler(
        IDispenserTypeCreateDispenserType dispenserTypeDomainService,
        IMapper mapper) : IRequestHandler<CreateDispenserTypeCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateDispenserTypeCommand request, CancellationToken cancellationToken)
        {
            var dispenserTypeEntity = mapper.Map<DispenserTypeEntity>(request.Request);
            var result = await dispenserTypeDomainService.CreateAsync(dispenserTypeEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}



