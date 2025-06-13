using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Provider.Commands.UpdateProvider;

public class UpdateProviderCommandHandler(IProviderUpdateService ProviderUpdateService, IMapper mapper) : IRequestHandler<UpdateProviderCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(UpdateProviderCommand request,
        CancellationToken cancellationToken)
    {
        var ProviderEntity = mapper.Map<ProviderEntity>(request);
        var result = await ProviderUpdateService.UpdateAsync(ProviderEntity);

        if (!result.IsSuccess)
            return result.Error!;
        return result.Value!;
    }
}