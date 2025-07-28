using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class GetIdAuxService(ITenantDbContextFactory dbContextFactory) : IGetProductAndCompartiment, IGetExpenditureId, IGetTypeOfCollectionId
{
    public async Task<ProductAndCompartimentEntity> GetProductAndCompartimentAsync(int hoseId)
    {

        using var context = dbContextFactory.CreateDbContext();
        var result = await context.Hose
            .Where(h => h.IdHose == hoseId)
            .Select(h => new
            {
                IdProduct = context.Product
                    .Where(p => p.IdProductType == h.IdProductType)
                    .Select(p => (int)p.IdProduct)
                    .FirstOrDefault(),
                IdCompartiment = context.ProductCompartiment
                    .Where(pc => pc.IdProduct == context.Product
                        .Where(p => p.IdProductType == h.IdProductType)
                        .Select(p => p.IdProduct)
                        .FirstOrDefault())
                    .Select(pc => (int)pc.IdCompartiment)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new InvalidOperationException($"No se encontraron datos para el HoseId: {hoseId}");
        }

        return new ProductAndCompartimentEntity(result.IdProduct, result.IdCompartiment);
    }

    public async Task<int?> GetExpenditureIdAsync(string expenditureName)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Expenditures
            .Where(e => e.Description == expenditureName)
            .Select(e => (int?)e.IdExpenditures)
            .FirstOrDefaultAsync();
    }

    public async Task<int?> GetTypeOfCollectionIdAsync(string typeOfCollectionName)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.TypeOfCollection
            .Where(tc => tc.Description == typeOfCollectionName)
            .Select(tc => (int?)tc.IdTypeOfCollection)
            .FirstOrDefaultAsync();
    }
}
