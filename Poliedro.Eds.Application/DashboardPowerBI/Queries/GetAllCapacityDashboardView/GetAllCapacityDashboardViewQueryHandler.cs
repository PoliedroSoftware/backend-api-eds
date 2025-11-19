using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CapacityDashboardView;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.DomainCapacityDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCapacityDashboardView;

public class GetAllCapacityDashboardViewQueryHandler(
    ICapacityDashboardViewGetAllService capacityDashboardViewGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllCapacityDashboardViewQueryHandler> logger,
    IMapper mapper)
    : IRequestHandler<GetAllCapacityDashboardViewQuery, IEnumerable<CapacityDashboardViewDto>>
{
    public async Task<IEnumerable<CapacityDashboardViewDto>> Handle(
        GetAllCapacityDashboardViewQuery request, 
        CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var cacheKey = $"capacityDashboardView:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<CapacityDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return mapper.Map<IEnumerable<CapacityDashboardViewDto>>(cachedData);
        }

        var data = await capacityDashboardViewGetAllService.GetAllAsync(request.PaginationParams);

        logger.LogInformation("Retrieved {Count} capacity dashboard view records", data.Count());

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CapacityDashboardViewDto>>(data);
    }
}
