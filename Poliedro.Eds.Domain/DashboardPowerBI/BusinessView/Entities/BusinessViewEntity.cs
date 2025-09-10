using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;

public class BusinessViewEntity
{
    [Key]
    public int IdBusiness { get; set; } = default!;
    public string BusinessName { get; set; } = default!;
    public string EdsName { get; set; } = default!;
    public string TankNumber { get; set; } = default!;
    public int CompartimentNumber { get; set; } = default!;
    public int IdProduct { get; set; } = default!;    
    public string ProductName { get; set; } = default!;
    public DateOnly ProductDate { get; set; } = default!;
}
