using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GetTypeOfCollectionById;

public class GetTypeOfCollectionByIdQueryHandler(
    ITypeOfCollectionGetByIdTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
    IMapper mapper,
    IValidator<GetTypeOfCollectionByIdQuery> validator)
    : IRequestHandler<GetTypeOfCollectionByIdQuery, Result<TypeOfCollectionDto, Error>>
{
    public async Task<Result<TypeOfCollectionDto, Error>> Handle(GetTypeOfCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<TypeOfCollectionDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }
        var result = await TypeOfCollectionDomainTypeOfCollection.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        return mapper.Map<TypeOfCollectionDto>(result.Value);
    }
}
