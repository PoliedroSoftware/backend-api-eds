using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;

public class ProductViewEntity
{
    [Key]
    public int IdProduct { get; set; } = default!;
    public int IdBusiness { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int IdProductType { get; set; }
    public double Price { get; set; }
    public DateOnly Date { get; set; } = default!;
}
