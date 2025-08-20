using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

public class ProductCompartimentGetByCompartmentId(ITenantDbContextFactory dbContextFactory) : IProductCompartimentGetByCompartmentId
{
    public async Task<int?> GetProductIdByCompartmentIdAsync(int idCompartment)
    {
        using var context = dbContextFactory.CreateDbContext();
        var entity = await context.ProductCompartiment
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdCompartiment == idCompartment);

        return entity?.IdProduct;
    }
}
