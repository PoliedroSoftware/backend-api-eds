using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GellAllHose;

public class GetAllHoseQueryHandler
(
    IHoseGetAllHose hoseDomainHose,
    IMapper mapper)
    : IRequestHandler<GellAllHoseQuery, Result<IEnumerable<HoseDto>, Error>>
{
    public async Task<Result<IEnumerable<HoseDto>, Error>> Handle(GellAllHoseQuery request, CancellationToken cancellationToken)
    {
        var result = await hoseDomainHose.GetAllAsync(request.PaginationParams);
        
        if (!result.IsSuccess)
            return result.Error!;

        return Result<IEnumerable<HoseDto>, Error>.Success(result.Value!);
    }
}

