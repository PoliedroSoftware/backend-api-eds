using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.ProductType.DomainProductType;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Application.ProductType.Queries.GellAllProductType;
public class GetAllProductTypeQueryHandler
(
    IProductTypeGetAllProductType ProductTypeGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllProductTypeQuery, IEnumerable<ProductTypeDto>>
{
    public async Task<IEnumerable<ProductTypeDto>> Handle(GellAllProductTypeQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"productType:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProductTypeEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<ProductTypeDto>>(cachedData);
        }

        var data = await ProductTypeGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<ProductTypeDto>>(data);
    }
}


