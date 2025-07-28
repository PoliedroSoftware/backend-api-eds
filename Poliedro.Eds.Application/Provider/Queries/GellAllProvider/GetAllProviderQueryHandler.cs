using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Application.Provider.Queries.GellAllProvider;

public class GetAllProviderQueryHandler
(
    IProviderGetAllService ProviderGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllProviderQuery, IEnumerable<ProviderDto>>
{
    public async Task<IEnumerable<ProviderDto>> Handle(GellAllProviderQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"provider:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProviderEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<ProviderDto>>(cachedData);
        }

        var data = await ProviderGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<ProviderDto>>(data);
    }
}