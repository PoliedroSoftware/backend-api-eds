using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CompartimentDashboardView;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.DomainCompartimentDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.Entities;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCompartimentDashboardView;

public class GetAllCompartimentDashboardViewQueryHandler(
    ICompartimentDashboardViewGetAllService compartimentDashboardViewGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllCompartimentDashboardViewQueryHandler> logger,
    IMapper mapper)
    : IRequestHandler<GetAllCompartimentDashboardViewQuery, IEnumerable<CompartimentDashboardViewDto>>
{
    public async Task<IEnumerable<CompartimentDashboardViewDto>> Handle(
        GetAllCompartimentDashboardViewQuery request, 
        CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var cacheKey = $"compartimentDashboardView:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return mapper.Map<IEnumerable<CompartimentDashboardViewDto>>(cachedData);
        }

        var data = await compartimentDashboardViewGetAllService.GetAllAsync(request.PaginationParams);

        logger.LogInformation("Retrieved {Count} compartiment dashboard view records", data.Count());

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CompartimentDashboardViewDto>>(data);
    }
}
