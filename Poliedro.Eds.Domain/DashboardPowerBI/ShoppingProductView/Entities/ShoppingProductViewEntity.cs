using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;

public class ShoppingProductViewEntity
{
    [Key]
    public int? IdShoppingProduct { get; set; }
    public int? IdShopping { get; set; }
    public int? IdProduct { get; set; }
    public int? IdBusiness { get; set; }
    public double? Quantity { get; set; }
    public double? Price { get; set; }
    public double? TotalPrice { get; set; }
    public DateOnly? Date { get; set; }
    public string? ProductName { get; set; } = string.Empty;
    public int? IdCompartment { get; set; }
}


