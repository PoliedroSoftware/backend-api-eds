using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PointOfSale.Entities;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetAllPosOfSale;

public class GetAllPosOfSaleQueryHandler
(
    IPosOfSaleGetAllService posOfSaleDomain,
    IMapper mapper)
    : IRequestHandler<GellAllPosOfSaleQuery, Result<IEnumerable<PosOfSaleEntity>, Error>>
{
    public async Task<Result<IEnumerable<PosOfSaleEntity>, Error>> Handle(GellAllPosOfSaleQuery request, CancellationToken cancellationToken)
    {
        var result = await posOfSaleDomain.GetAllAsync(request.PaginationParams);

        if (!result.IsSuccess)
            return result.Error!;

        return Result<IEnumerable<PosOfSaleEntity>, Error>.Success(result.Value!);
    }
}
