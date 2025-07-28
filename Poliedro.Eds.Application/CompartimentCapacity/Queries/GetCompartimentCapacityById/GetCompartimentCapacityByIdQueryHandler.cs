using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using System.Net;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GetCompartimentCapacityById;

public class GetCompartimentCapacityByIdQueryHandler(
    ICompartimentCapacityGetByIdService CompartimentCapacityDomainCompartimentCapacity,
    IMapper mapper,
    IValidator<GetCompartimentCapacityByIdQuery> validator)
    : IRequestHandler<GetCompartimentCapacityByIdQuery, Result<CompartimentCapacityDto, Error>>
{
    public async Task<Result<CompartimentCapacityDto, Error>> Handle(GetCompartimentCapacityByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<CompartimentCapacityDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await CompartimentCapacityDomainCompartimentCapacity.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<CompartimentCapacityDto>(result.Value);
    }
}