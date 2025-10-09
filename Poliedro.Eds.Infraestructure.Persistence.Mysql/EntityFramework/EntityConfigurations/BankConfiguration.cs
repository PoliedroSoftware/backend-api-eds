using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Bank.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class BankConfiguration
{
    public BankConfiguration(EntityTypeBuilder<BankEntity> builder)
    {
        builder.ToTable("bank");
        builder.HasKey(x => x.IdBank);
        builder.Property(x => x.IdBank).HasColumnName("id_bank");
        builder.Property(x => x.IdAccount).HasColumnName("id_account");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.IdCourt).HasColumnName("id_court");
        builder.Property(x => x.Moviment).HasColumnName("moviment").HasMaxLength(100);
        builder.Property(x => x.Note).HasColumnName("note").HasMaxLength(500);
        builder.Property(x => x.Ammount).HasColumnName("ammount");
        builder.Property(x => x.Balance).HasColumnName("balance");
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy").HasMaxLength(100);
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy").HasMaxLength(255);
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
        
        // Configurar las relaciones de foreign key
        builder.HasIndex(x => x.IdAccount).HasDatabaseName("bank_account_FK");
        builder.HasIndex(x => x.IdEds).HasDatabaseName("bank_eds_FK");
    }
}
