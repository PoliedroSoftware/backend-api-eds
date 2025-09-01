using AutoMapper;
using MediatR;
<<<<<<< HEAD
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
=======
>>>>>>> New-service-StrongBox
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GellAllHose;

public class GetAllHoseQueryHandler
(
    IHoseGetAllHose hoseDomainHose,
    IMapper mapper)
<<<<<<< HEAD
    : IRequestHandler<GellAllHoseQuery, Result<IEnumerable<HoseDto>, Error>>
{
    public async Task<Result<IEnumerable<HoseDto>, Error>> Handle(GellAllHoseQuery request, CancellationToken cancellationToken)
    {
        var result = await hoseDomainHose.GetAllAsync(request.PaginationParams);
        
        if (!result.IsSuccess)
            return result.Error!;

        return Result<IEnumerable<HoseDto>, Error>.Success(result.Value!);
=======
    : IRequestHandler<GellAllHoseQuery, IEnumerable<HoseDto>>
{
    public async Task<IEnumerable<HoseDto>> Handle(GellAllHoseQuery request, CancellationToken cancellationToken)
    {
        var result = await hoseDomainHose.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<HoseDto>>(result);
>>>>>>> New-service-StrongBox
    }
}

