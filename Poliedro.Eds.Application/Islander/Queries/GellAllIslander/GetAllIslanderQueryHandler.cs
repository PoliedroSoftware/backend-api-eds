using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Application.Islander.Queries.GellAllIslander;

public class GetAllIslanderQueryHandler
(
    IIslanderGetAllIslander IslanderGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllIslanderQuery, IEnumerable<IslanderDto>>
{
    public async Task<IEnumerable<IslanderDto>> Handle(GellAllIslanderQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"islander:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<IslanderEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<IslanderDto>>(cachedData);
        }

        var data = await IslanderGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<IslanderDto>>(data);
    }
}

