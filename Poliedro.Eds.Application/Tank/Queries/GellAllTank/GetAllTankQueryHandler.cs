using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.Queries.GellAllTank;

public class GetAllTankQueryHandler
(
    ITankGetAllTank TankGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllTankQuery, IEnumerable<TankDto>>
{
    public async Task<IEnumerable<TankDto>> Handle(GellAllTankQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"tank:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<TankEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<TankDto>>(cachedData);
        }

        var data = await TankGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<TankDto>>(data);
    }
}

