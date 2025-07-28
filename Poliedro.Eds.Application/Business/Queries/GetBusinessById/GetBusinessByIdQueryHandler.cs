using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Queries.GetBusinessById;

public class GetBusinessByIdQueryHandler(
    IBusinessGetByIdService BusinessGetByIdService,
    IMapper mapper,
    IValidator<GetBusinessByIdQuery> validator)
    : IRequestHandler<GetBusinessByIdQuery, Result<BusinessDto, Error>>
{
    public async Task<Result<BusinessDto, Error>> Handle(GetBusinessByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<BusinessDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await BusinessGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<BusinessDto>(result.Value);
    }
}
