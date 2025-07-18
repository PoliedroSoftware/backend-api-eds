using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProduct.Queries.GellAllShoppingProduct;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.DomainShoppingProduct;
using Poliedro.Eds.Domain.ShoppingProductInventory.DomainShoppingProductInventory;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GellAllShoppingProductInventory
{
    public class GetAllShoppingProductInventoryQueryHandler(
        IShoppingProductInventoryGetAll ShoppingProductInventoryGetAllService,
        IRedisService redisService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
        ) : IRequestHandler<GellAllShoppingProductInventoryQuery, IEnumerable<ShoppingProductInventoryDto>>
    {
        public async Task<IEnumerable<ShoppingProductInventoryDto>> Handle(GellAllShoppingProductInventoryQuery request, CancellationToken cancellationToken)
        {
            var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
            var cacheKey = $"shoppingProductInventory:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
            var cachedData = await redisService.GetCacheAsync<IEnumerable<ShoppingProductInventoryEntity>>(cacheKey);
            if (cachedData is not null)
            {
                return mapper.Map<IEnumerable<ShoppingProductInventoryDto>>(cachedData);
            }

            var data = await ShoppingProductInventoryGetAllService.GetAllAsync(request.PaginationParams);

            await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

            return mapper.Map<IEnumerable<ShoppingProductInventoryDto>>(data);
        }
    }
}
