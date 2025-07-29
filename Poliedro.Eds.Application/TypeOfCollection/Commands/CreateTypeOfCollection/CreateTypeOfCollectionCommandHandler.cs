using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;

public class CreateTypeOfCollectionCommandHandler(
    ITypeOfCollectionCreateTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
    IMapper mapper,
        IValidator<CreateTypeOfCollectionRequestDto> validator,
        IRedisService redisService
    ) : IRequestHandler<CreateTypeOfCollectionCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateTypeOfCollectionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

        var result = await TypeOfCollectionDomainTypeOfCollection.CreateAsync(mapper.Map<TypeOfCollectionEntity>(request.Request));
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.TYPE_OF_COLLECTION);
        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
