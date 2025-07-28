using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Application.Eds.Queries.GellAllEds;

public class GetAllEdsQueryHandler
(
    IEdsGetAllService EdsGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllEdsQuery, IEnumerable<EdsDto>>
{
    public async Task<IEnumerable<EdsDto>> Handle(GellAllEdsQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"eds:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<EdsEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<EdsDto>>(cachedData);
        }

        var data = await EdsGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<EdsDto>>(data);
    }
}