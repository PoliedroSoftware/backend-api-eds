using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ShoppingCreateShopping(DataBaseContext context, IRedisService redisService) : IShoppingCreateShopping
{
    public async Task<Result<VoidResult, Error>> CreateAsync(ShoppingEntity shoppingEntity)
    {
        await context.Shopping.AddAsync(shoppingEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return ShoppingErrorBuilder.ShoppingCreationException();
        await redisService.RemoveByPrefixAsync("shopping:");

        return VoidResult.Instance;
    }
}