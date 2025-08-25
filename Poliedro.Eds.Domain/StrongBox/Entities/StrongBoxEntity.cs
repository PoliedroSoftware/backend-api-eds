using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;

namespace Poliedro.Eds.Domain.StrongBox.Entities
{
    [Table("strongbox")]
    public class StrongBoxEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Auto-increment
        public long Id { get; private set; }

        [Column("datetime")]
        public DateTime DateTime { get; private set; }

        [Column("id_corte")]
        public long? IdCorte { get; private set; }

        [Column("moviment")]
        [MaxLength(10)]
        public string Type { get; private set; } = null!;

        [Column("ammount")]
        public decimal Ammount { get; private set; }

        [Column("saldo")]
        public decimal Saldo { get; private set; }

        [Column("note")]
        [MaxLength(500)]
        public string? Note { get; private set; }

        [Column("createdBy")]
        [MaxLength(50)]
        public string CreatedBy { get; private set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [Column("updatedBy")]
        [MaxLength(100)]
        public string? UpdatedBy { get; private set; }

        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; private set; }

        private StrongBoxEntity() { }

        public StrongBoxEntity(DateTime dateTime, long? idCorte, string type, decimal ammount, decimal saldo, string? note)
        {
            if (string.IsNullOrWhiteSpace(type))
            { 
            throw new ArgumentException("Invalid strong box type", nameof(type));
            }
            type = type.Trim().ToUpperInvariant();

            if(!StrongBoxType.IsValid(type))
            {
                throw new ArgumentException("Invalid strong box type", nameof(type));
            }

            if (ammount <=0)
            {
                throw new ArgumentException("Ammount debe ser mayor a 0", nameof(ammount));
            }

            DateTime = dateTime == default ? DateTime.UtcNow : dateTime;
            IdCorte = idCorte;
            Type = type;
            Ammount = decimal.Round(ammount, 2);
            Saldo = 0m;
            Note = note?.Trim();
            CreatedAt = DateTime.UtcNow;
        }

        public void SetSaldo(decimal nuevoSaldo)
        {
            Saldo = decimal.Round(nuevoSaldo, 2);
            UpdatedAt = DateTime.UtcNow;
            
        }

    }
}
