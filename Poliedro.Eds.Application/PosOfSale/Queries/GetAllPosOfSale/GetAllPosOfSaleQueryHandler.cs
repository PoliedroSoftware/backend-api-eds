using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Hose.Queries.GellAllHose;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetAllPosOfSale;

public class GetAllPosOfSaleQueryHandler
(
    IPosOfSaleGetAll posOfSaleDomain,
    IMapper mapper)
    : IRequestHandler<GellAllPosOfSaleQuery, Result<IEnumerable<PosOfSaleDto>, Error>>
{
    public async Task<Result<IEnumerable<PosOfSaleDto>, Error>> Handle(GellAllPosOfSaleQuery request, CancellationToken cancellationToken)
    {
        var result = await posOfSaleDomain.GetAllAsync(request.PaginationParams);

        if (!result.IsSuccess)
            return result.Error!;

        return Result<IEnumerable<PosOfSaleDto>, Error>.Success(result.Value!);
    }
}
