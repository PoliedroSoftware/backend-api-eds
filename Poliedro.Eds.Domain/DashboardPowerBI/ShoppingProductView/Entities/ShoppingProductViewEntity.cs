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
    public double Quantity { get; set; }
    public double SellPrice { get; set; }
    public double PurchasePrice { get; set; }
    public double Stock { get; set; }
    public int IdCompartment { get; set; }
    public double TotalSellPrice { get; set; }
    public double TotalPurchasePrice { get; set; }
    public int IdBusiness { get; set; }
}
