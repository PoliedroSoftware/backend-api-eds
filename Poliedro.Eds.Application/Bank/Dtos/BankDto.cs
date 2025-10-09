using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Application.Bank.Dtos;

public class BankDto 
{
    public int IdBank { get; set; }
    public int IdAccount { get; set; }
    public int IdEds { get; set; }
    public int? IdCourt { get; set; }
    public string Moviment { get; set; } = null!;
    public string Note { get; set; } = null!;
    public double Ammount { get; set; }
    public double Balance { get; set; }
}
