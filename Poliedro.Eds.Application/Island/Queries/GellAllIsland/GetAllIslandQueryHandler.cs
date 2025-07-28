using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Application.Island.Queries.GellAllIsland;

public class GetAllIslandQueryHandler
(
    IIslandGetAllIsland IslandGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllIslandQuery, IEnumerable<IslandDto>>
{
    public async Task<IEnumerable<IslandDto>> Handle(GellAllIslandQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"island:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<IslandEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<IslandDto>>(cachedData);
        }

        var data = await IslandGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<IslandDto>>(data);
    }
}

