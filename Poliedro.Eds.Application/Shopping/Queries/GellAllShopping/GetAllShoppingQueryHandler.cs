using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Application.Shopping.Queries.GellAllShopping;
public class GetAllShoppingQueryHandler
(
    IShoppingGetAllShopping ShoppingGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllShoppingQuery, IEnumerable<ShoppingDto>>
{
    public async Task<IEnumerable<ShoppingDto>> Handle(GellAllShoppingQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"shopping:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ShoppingEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<ShoppingDto>>(cachedData);
        }

        var data = await ShoppingGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<ShoppingDto>>(data);
    }
}


