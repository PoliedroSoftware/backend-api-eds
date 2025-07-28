using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.DispenserType.DomainDispenserType;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Application.DispenserType.Queries.GellAllDispenserType;

public class GetAllDispenserTypeQueryHandler
(
    IDispenserTypeGetAllDispenserType DispenserTypeGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllDispenserTypeQuery, IEnumerable<DispenserTypeDto>>
{
    public async Task<IEnumerable<DispenserTypeDto>> Handle(GellAllDispenserTypeQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"dispenserType:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<DispenserTypeEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<DispenserTypeDto>>(cachedData);
        }

        var data = await DispenserTypeGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<DispenserTypeDto>>(data);
    }
}


