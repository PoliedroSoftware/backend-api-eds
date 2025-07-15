using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;

public class TypeOfCollectionViewEntity
{
    [Key]
    public int IdTypeOfCollection { get; set; }
    public int IdProduct { get; set; } 
    public int? IdBusiness { get; set; }
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
}