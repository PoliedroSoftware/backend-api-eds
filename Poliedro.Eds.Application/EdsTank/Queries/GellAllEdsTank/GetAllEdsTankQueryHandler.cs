using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Application.EdsTank.Queries.GellAllEdsTank;

public class GetAllEdsTankQueryHandler
(
    IEdsTankGetAllEdsTank EdsTankGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllEdsTankQuery, IEnumerable<EdsTankDto>>
{
    public async Task<IEnumerable<EdsTankDto>> Handle(GellAllEdsTankQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"edsTank:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<EdsTankEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<EdsTankDto>>(cachedData);
        }

        var data = await EdsTankGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<EdsTankDto>>(data);
    }
}


