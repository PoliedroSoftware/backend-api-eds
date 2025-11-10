using Poliedro.Eds.Application.Product.Dtos;

namespace Poliedro.Eds.Application.Shopping.Dtos;

public class ShoppingProductDto
{
    public int IdShoppingProduct { get; set; }
    public int IdShopping { get; set; }
    public int IdProduct { get; set; }
    public double Quantity { get; set; }
    public double PurchasePrice { get; set; }
    public double SellPrice { get; set; }
    public int IdCompartment { get; set; }
    
    // Related entity
    public ProductDto? Product { get; set; }
}
