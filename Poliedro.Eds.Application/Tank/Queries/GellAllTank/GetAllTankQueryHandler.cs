using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Services.Cache;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Tank.DomainTank;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Application.Tank.Queries.GellAllTank;

public class GetAllTankQueryHandler
(
    ITankGetAllTank TankGetAllService,
    ICacheService cacheService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllTankQuery, IEnumerable<TankDto>>
{
    public async Task<IEnumerable<TankDto>> Handle(GellAllTankQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString() ?? "default";
        var cacheKey = $"tank:list:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}";
        

        var cacheTags = new[] { "tank", "edstank", "compartiment" };
        
        var data = await cacheService.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var entities = await TankGetAllService.GetAllAsync(request.PaginationParams);
                return mapper.Map<IEnumerable<TankDto>>(entities);
            },
            TimeSpan.FromMinutes(1440), 
            cacheTags,
            cancellationToken);

        return data;
    }
}

