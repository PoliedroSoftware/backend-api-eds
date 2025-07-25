using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.Capacity.Queries.GetCapacityById;
public class GetCapacityByIdQueryHandler(
    ICapacityGetByIdService CapacityGetByIdService,
    IMapper mapper,
    IValidator<GetCapacityByIdQuery> validator)
    : IRequestHandler<GetCapacityByIdQuery, Result<CapacityDto, Error>>
{
    public async Task<Result<CapacityDto, Error>> Handle(GetCapacityByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<CapacityDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await CapacityGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<CapacityDto>(result.Value);
    }
}