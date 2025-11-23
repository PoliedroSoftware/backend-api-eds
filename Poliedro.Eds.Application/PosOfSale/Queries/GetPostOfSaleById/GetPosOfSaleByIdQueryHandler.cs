using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Hose.Queries.GetHoseById;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetPostOfSaleById
{
    public class GetPosOfSaleByIdQueryHandler(
       IPosOfSaleGetById posOfSaleDomain,
       IMapper mapper,
       IValidator<GetPosOfSaleByIdQuery> validator)
       : IRequestHandler<GetPosOfSaleByIdQuery, Result<HoseDto, Error>>
    {
        public async Task<Result<PosOfSaleDto, Error>> Handle(GetPosOfSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<PosOfSaleDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await posOfSaleDomain.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<PosOfSaleDto>(result.Value);
        }
    }
}
