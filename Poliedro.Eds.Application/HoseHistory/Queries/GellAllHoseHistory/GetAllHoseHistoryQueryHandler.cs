using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;
using Poliedro.Eds.Domain.HoseHistory.Entities;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GellAllHoseHistory;

public class GetAllHoseHistoryQueryHandler
(
    IHoseHistoryGetAllHoseHistory HoseHistoryGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllHoseHistoryQuery, IEnumerable<HoseHistoryDto>>
{
    public async Task<IEnumerable<HoseHistoryDto>> Handle(GellAllHoseHistoryQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"hoseHistory:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<HoseHistoryEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<HoseHistoryDto>>(cachedData);
        }

        var data = await HoseHistoryGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<HoseHistoryDto>>(data);
    }
}

