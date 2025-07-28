using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Capacity.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;

namespace Poliedro.Eds.Application.Capacity.Queries.GellAllCapacity;

public class GetAllCapacityQueryHandler
(
    ICapacityGetAllService CapacityGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllCapacityQuery, IEnumerable<CapacityDto>>
{
    public async Task<IEnumerable<CapacityDto>> Handle(GellAllCapacityQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"capacity:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        // Buscar en cache
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CapacityEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<CapacityDto>>(cachedData);
        }

        // Si no se encuentra en cache, consultar la base de datos
        var data = await CapacityGetAllService.GetAllAsync(request.PaginationParams);

        // Almacenar en cache
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CapacityDto>>(data);
    }
}