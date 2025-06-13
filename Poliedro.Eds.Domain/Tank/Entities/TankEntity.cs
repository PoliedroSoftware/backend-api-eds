using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Tank.Entities;

public class TankEntity
{
    [Key]
    public int IdTank { get; set; }
    public string Number { get; set; }
    public int Compartment { get; init; }
    public double Ability { get; init; }
    public double Stock { get; init; }

}
