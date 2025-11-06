using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;

public class ShoppingProductViewEntity
{
    [Key]
    public int IdShoppingProduct { get; set; }
    public int IdShopping { get; set; }
    public int IdProduct { get; set; }
    public DateOnly Date { get; set; }
    public string ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal SellPrice { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Stock { get; set; }
    public int IdCompartment { get; set; }
    public decimal TotalSellPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public int IdBusiness { get; set; }
}
