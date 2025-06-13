using Poliedro.Eds.Domain.ProductCompartiment.DomainProductCompartiment;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.EntityFrameworkCore;

public class ProductCompartimentGetByCompartmentId : IProductCompartimentGetByCompartmentId
{
    private readonly DataBaseContext _context;

    public ProductCompartimentGetByCompartmentId(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<int?> GetProductIdByCompartmentIdAsync(int idCompartment)
    {
        var entity = await _context.ProductCompartiment
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdCompartiment == idCompartment);

        return entity?.IdProduct;
    }
}
