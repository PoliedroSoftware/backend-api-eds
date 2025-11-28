using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.DomainPosOfSaleDetails;
using Poliedro.Eds.Domain.PosOfSaleDetails.Dtos;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Queries.GetPostOfSaleById
{
    public class GetPosOfSaleDetailsByIdQueryHandler(
       IPosOfSaleDetailsGetByIdService posOfSaleDetailsDomain,
       IMapper mapper,
       IValidator<GetPosOfSaleDetailsByIdQuery> validator)
       : IRequestHandler<GetPosOfSaleDetailsByIdQuery, Result<PosOfSaleDetailsDto, Error>>
    {
        public async Task<Result<PosOfSaleDetailsDto, Error>> Handle(GetPosOfSaleDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<PosOfSaleDetailsDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }
            var result = await posOfSaleDetailsDomain.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<PosOfSaleDetailsDto>(result.Value);
        }
    }
}
