using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Queries.GetEdsById;

public class GetEdsByIdQueryHandler(
    IEdsGetByIdService edsGetByIdService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    IValidator<GetEdsByIdQuery> validator)
    : IRequestHandler<GetEdsByIdQuery, Result<EdsDto, Error>>
{
    public async Task<Result<EdsDto, Error>> Handle(GetEdsByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<EdsDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var cacheKey = $"{KeyRedisConstants.EDS}{request.Id}:{tenant}";
        
        var cachedData = await redisService.GetCacheAsync<EdsEntity>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<EdsDto>(cachedData);
        }

        var result = await edsGetByIdService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        await redisService.SetCacheAsync(cacheKey, result.Value, TimeSpan.FromMinutes(1440));

        return mapper.Map<EdsDto>(result.Value);
    }
}
