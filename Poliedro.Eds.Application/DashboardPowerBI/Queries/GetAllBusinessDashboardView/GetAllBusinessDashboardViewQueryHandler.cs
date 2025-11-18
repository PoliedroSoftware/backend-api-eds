using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDashboardView;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.DomainBusinessDashboardView;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.Entities;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllBusinessDashboardView;

public class GetAllBusinessDashboardViewQueryHandler(
    IBusinessDashboardViewGetAllService businessDashboardViewGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllBusinessDashboardViewQueryHandler> logger,
    IMapper mapper)
    : IRequestHandler<GetAllBusinessDashboardViewQuery, IEnumerable<BusinessDashboardViewDto>>
{
    public async Task<IEnumerable<BusinessDashboardViewDto>> Handle(
        GetAllBusinessDashboardViewQuery request, 
        CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var cacheKey = $"businessDashboardView:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<BusinessDashboardViewEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return mapper.Map<IEnumerable<BusinessDashboardViewDto>>(cachedData);
        }

        var data = await businessDashboardViewGetAllService.GetAllAsync(request.PaginationParams);

        logger.LogInformation("Retrieved {Count} business dashboard view records", data.Count());

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<BusinessDashboardViewDto>>(data);
    }
}
