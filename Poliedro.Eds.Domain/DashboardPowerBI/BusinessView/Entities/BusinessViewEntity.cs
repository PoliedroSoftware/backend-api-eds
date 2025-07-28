using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;

public class BusinessViewEntity
{
    [Key]
    public int IdBusiness { get; set; } = default!;
    public string IdTank { get; set; } = default!;
    public int IdProduct { get; set; } = default!;
    public int IdCompartiment { get; set; } = default!;
    public string NameEds { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
}
