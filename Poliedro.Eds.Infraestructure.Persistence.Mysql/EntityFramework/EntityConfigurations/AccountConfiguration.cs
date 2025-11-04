using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class AccountConfiguration
{
    public AccountConfiguration(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("account");
        builder.HasKey(x => x.IdAccount);
        builder.Property(x => x.IdAccount).HasColumnName("id_account");
        builder.Property(x => x.AccountType).HasColumnName("account_type").HasMaxLength(100);
        builder.Property(x => x.Bank).HasColumnName("bank").HasMaxLength(100);
        builder.Property(x => x.Account).HasColumnName("account").HasMaxLength(100);
        builder.Property(x => x.Holder).HasColumnName("holder").HasMaxLength(100);
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy").HasMaxLength(255);
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy").HasMaxLength(255);
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");

        // mapeo id_eds
        builder.Property(x => x.IdEds).HasColumnName("id_eds").IsRequired(false);
        builder.HasIndex(x => x.IdEds).HasDatabaseName("idx_account_id_eds");
        builder.HasOne<EdsEntity>()
               .WithMany()
               .HasForeignKey(x => x.IdEds)
               .OnDelete(DeleteBehavior.Restrict);

        // índice único existente
        builder.HasIndex(x => x.Account).IsUnique().HasDatabaseName("account_unique");
    }
}
