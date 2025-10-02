using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;

namespace Poliedro.Eds.Domain.StrongBox.Entities;

public class StrongBoxEntity : AuditableEntity
{
    public long Id { get; set; }
    public long? IdCorte { get; set; }
    public int? IdEds { get; set; }
    public string Type { get; set; } = null!;
    public double Ammount { get; set; }
    public double Saldo { get; set; }
    public string? Note { get; set; }

    private StrongBoxEntity() { }

    public StrongBoxEntity(
        long? idCorte,
        int? idEds,
        string type,
        double ammount,
        double saldo,
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
        IdEds = idEds;
        Type = type;
        Ammount = double.Round(ammount, 2);
        Saldo = double.Round(saldo, 2);
        Note = note?.Trim();
    }

    public void SetSaldo(double nuevoSaldo)
    {
        Saldo = double.Round(nuevoSaldo, 2);
        UpdatedAt = DateTime.Now;

    }

}
