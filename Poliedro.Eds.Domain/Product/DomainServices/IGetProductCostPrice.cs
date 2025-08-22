using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.Product.DomainServices;

public interface IGetProductCostPrice
{
    Task<double?> GetProductCostPriceAsync(int idProduct);
}
