using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Bank.ValueObjects;

namespace Poliedro.Eds.Domain.Bank.Entities;

public class BankEntity : AuditableEntity
{
    [Key]
    public int IdBank { get; set; }
    public int IdAccount { get; set; }
    public int IdEds { get; set; }
    public int? IdCourt { get; set; }
    public string Moviment { get; set; } = null!;
    public string Note { get; set; } = null!;
    public double Ammount { get; set; }
    public double Balance { get; set; }

    private BankEntity() { }

    public BankEntity(
        int idAccount,
        int idEds,
        int? idCourt,
        string moviment,
        string note,
        double ammount,
        double balance)
    {
        if (string.IsNullOrWhiteSpace(moviment))
        {
            throw new ArgumentException("Invalid movement type", nameof(moviment));
        }
        
        moviment = moviment.Trim().ToUpperInvariant();

        if (!BankMovementType.IsValid(moviment))
        {
            throw new ArgumentException("Invalid movement type", nameof(moviment));
        }

        if (ammount <= 0)
        {
            throw new ArgumentException("Amount debe ser mayor a 0", nameof(ammount));
        }

        if (string.IsNullOrWhiteSpace(note))
        {
            throw new ArgumentException("Note is required", nameof(note));
        }

        IdAccount = idAccount;
        IdEds = idEds;
        IdCourt = idCourt;
        Moviment = moviment;
        Note = note.Trim();
        Ammount = double.Round(ammount, 2);
        Balance = double.Round(balance, 2);
    }

    public void SetBalance(double newBalance)
    {
        Balance = double.Round(newBalance, 2);
        UpdatedAt = DateTime.Now;
    }
}
