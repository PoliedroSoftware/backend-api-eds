using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Shopping.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Shopping.DomainShopping.Impl;

public class ShoppingUpdateShopping(DataBaseContext context, IRedisService redisService) : IShoppingUpdateShopping
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(ShoppingEntity shoppingEntity)
    {
        if (!await EntityExists(shoppingEntity.IdShopping))
            return ShoppingErrorBuilder.ShoppingNotFoundException(shoppingEntity.IdShopping);

        context.Shopping.Update(shoppingEntity);

        if (await context.SaveChangesAsync() <= 0)
            return ShoppingErrorBuilder.ShoppingUpdateException();
        await redisService.RemoveByPrefixAsync("shopping:");

        return VoidResult.Instance;
    }
    private async Task<bool> EntityExists(int id)
    {
        return await context.Shopping
            .AsNoTracking()
            .AnyAsync(c => c.IdShopping == id);
    }
}