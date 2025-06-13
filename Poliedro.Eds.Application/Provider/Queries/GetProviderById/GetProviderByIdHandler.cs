using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Provider.DomainProvider;

namespace Poliedro.Eds.Application.Provider.Queries.GetProviderById;
public class GetProviderByIdQueryHandler(
    IProviderGetByIdService ProviderGetByIdService,
    IMapper mapper)
    : IRequestHandler<GetProviderByIdQuery, Result<ProviderDto, Error>>
{
    public async Task<Result<ProviderDto, Error>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await ProviderGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<ProviderDto>(result.Value);
    }
}