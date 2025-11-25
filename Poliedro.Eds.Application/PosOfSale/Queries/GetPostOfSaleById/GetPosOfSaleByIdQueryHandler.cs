using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSale.DomainPosOfSale;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetPostOfSaleById
{
    public class GetPosOfSaleByIdQueryHandler(
       IPosOfSaleGetByIdService posOfSaleDomain,
       IMapper mapper,
       IValidator<GetPosOfSaleByIdQuery> validator)
       : IRequestHandler<GetPosOfSaleByIdQuery, Result<PosOfSaleDto, Error>>
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
