using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.CourtDispensersInventory.DomainCourtDispensersInventory;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GellAllCourtDispensersInventory;

public class GetAllCourtDispensersInventoryQueryHandler
(
    ICourtDispensersInventoryGetAllCourtDispensersInventory CourtDispensersInventoryGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllCourtDispensersInventoryQuery, IEnumerable<CourtDispensersInventoryDto>>
{
    public async Task<IEnumerable<CourtDispensersInventoryDto>> Handle(GellAllCourtDispensersInventoryQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"courtDispensersInventory:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CourtDispensersInventoryEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<CourtDispensersInventoryDto>>(cachedData);
        }

        var data = await CourtDispensersInventoryGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CourtDispensersInventoryDto>>(data);
    }
}

