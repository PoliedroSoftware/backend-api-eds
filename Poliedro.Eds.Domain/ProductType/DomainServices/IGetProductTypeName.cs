namespace Poliedro.Eds.Domain.ProductType.DomainServices;

public interface IGetProductTypeName
{
    Task<string> GetProductTypeNameAsync(int idProductType);
}
