using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public class CreateProviderCommandHandler(
    IProviderCreateService ProviderCreateService,
    IMapper mapper) : IRequestHandler<CreateProviderCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var ProviderEntity = mapper.Map<ProviderEntity>(request.Request);
        var result = await ProviderCreateService.CreateAsync(ProviderEntity);
        if (!result.IsSuccess)
            return result.Error!;

        return result.Value!;
    }
}