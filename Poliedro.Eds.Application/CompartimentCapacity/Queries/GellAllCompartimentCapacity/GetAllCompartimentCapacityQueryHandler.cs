using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.CompartimentCapacity.DomainCompartimentCapacity;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;

namespace Poliedro.Eds.Application.CompartimentCapacity.Queries.GellAllCompartimentCapacity;
public class GetAllCompartimentCapacityQueryHandler
(
    ICompartimentCapacityGetAllService CompartimentCapacityGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllCompartimentCapacityQuery, IEnumerable<CompartimentCapacityDto>>
{
    public async Task<IEnumerable<CompartimentCapacityDto>> Handle(GellAllCompartimentCapacityQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"compartimentCapacity:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentCapacityEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<CompartimentCapacityDto>>(cachedData);
        }

        var data = await CompartimentCapacityGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CompartimentCapacityDto>>(data);
    }
}