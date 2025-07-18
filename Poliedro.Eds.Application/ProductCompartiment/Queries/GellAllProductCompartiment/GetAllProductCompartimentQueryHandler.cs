using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Domain.ProductCompartiment.Entities;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GellAllProductCompartiment;
public class GetAllProductCompartimentQueryHandler
(
    IProductCompartimentGetAllProductCompartiment ProductCompartimentGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllProductCompartimentQuery, IEnumerable<ProductCompartimentDto>>
{
    public async Task<IEnumerable<ProductCompartimentDto>> Handle(GellAllProductCompartimentQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"productCompartiment:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ProductCompartimentEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<ProductCompartimentDto>>(cachedData);
        }

        var data = await ProductCompartimentGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<ProductCompartimentDto>>(data);
    }
}


