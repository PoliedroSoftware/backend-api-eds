using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Dispensers.DomainDispensers;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Application.Dispensers.Queries.GellAllDispensers;
public class GetAllDispensersQueryHandler
(
    IDispensersGetAllDispensers DispensersGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllDispensersQuery, IEnumerable<DispensersDto>>
{
    public async Task<IEnumerable<DispensersDto>> Handle(GellAllDispensersQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"dispensers:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<DispensersEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<DispensersDto>>(cachedData);
        }

        var data = await DispensersGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<DispensersDto>>(data);
    }
}


