using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;

namespace Poliedro.Eds.Domain.StrongBox.Entities;

public class StrongBoxEntity : AuditableEntity
{
    public long Id { get; private set; }
    public long? IdCorte { get; private set; }
    public string Type { get; private set; } = null!;
    public decimal Ammount { get; private set; }
    public decimal Saldo { get; private set; }
    public string? Note { get; private set; }

    private StrongBoxEntity() { }

    public StrongBoxEntity(
        long? idCorte,
        string type,
        decimal ammount,
        decimal saldo,
        string? note)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Invalid strong box type", nameof(type));
        }
        type = type.Trim().ToUpperInvariant();

        if (!StrongBoxType.IsValid(type))
        {
            throw new ArgumentException("Invalid strong box type", nameof(type));
        }

        if (ammount <= 0)
        {
            throw new ArgumentException("Ammount debe ser mayor a 0", nameof(ammount));
        }

        IdCorte = idCorte;
        Type = type;
        Ammount = decimal.Round(ammount, 2);
        Saldo = decimal.Round(saldo, 2);
        Note = note?.Trim();
    }

    public void SetSaldo(decimal nuevoSaldo)
    {
        Saldo = decimal.Round(nuevoSaldo, 2);
        UpdatedAt = DateTime.Now;

    }

}
