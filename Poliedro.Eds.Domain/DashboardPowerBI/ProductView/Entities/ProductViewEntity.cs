using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;

public class ProductViewEntity
{
    [Key]
    public int IdProduct { get; set; }
    public int? IdBusiness { get; set; }
    public string Name { get; set; }
    public int IdProductType { get; set; }
    public double Price { get; set; }
    public DateOnly Date { get; set; }
}
