using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.BilligEds.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.BilligEds.DomainService;
using Poliedro.Eds.Domain.BilligEds.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Linq;
using Poliedro.Eds.Application.Common.Helper.removekey;

namespace Poliedro.Eds.Application.BilligEds.Commands.CreateBilligEds;

public class CreateBilligEdsCommandHandler(
    IBilligEdsCreateDomainService billigEdsCreateDomainService,
    IValidator<BillidEdsRequestEntity> validator,
    IRedisService redisService,
    IMapper mapper) : IRequestHandler<CreateBilligEdsCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreateBilligEdsCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request);
        if (!validationResult.IsValid)
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed", string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)), HttpStatusCode.BadRequest));

        var result = await billigEdsCreateDomainService.CreateBilligAsync(request.Request);

        // Optionally remove cache key if needed
        await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, "billigeds");

        return result.IsSuccess ? result.Value! : result.Error!;
    }
}
