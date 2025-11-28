using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Queries.GetAllPosOfSale;

public class GetAllPosOfSaleDetailsQueryHandler
(
    IPosOfSaleDetailsGetAllService posOfSaleDetailsDomain,
    IMapper mapper)
    : IRequestHandler<GellAllPosOfSaleDetailsQuery, Result<IEnumerable<PosOfSaleDetailsEntity>, Error>>
{
    public async Task<Result<IEnumerable<PosOfSaleDetailsEntity>, Error>> Handle(GellAllPosOfSaleDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await posOfSaleDetailsDomain.GetAllAsync(request.PaginationParams);

        if (!result.IsSuccess)
            return result.Error!;

        return Result<IEnumerable<PosOfSaleDetailsEntity>, Error>.Success(result.Value!);
    }
}
