using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.TransferValidation.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class TransferValidationConfiguration
{
    public TransferValidationConfiguration(EntityTypeBuilder<TransferValidationEntity> builder)
    {
        builder.ToTable("transfer_validation");

        builder.HasKey(x => x.IdTransferValidation);

        builder.Property(x => x.IdTransferValidation)
            .HasColumnName("id_transfer_validation")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UniqueId)
            .HasColumnName("unique_id")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CustomerName)
            .HasColumnName("customer_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.TransactionAmount)
            .HasColumnName("transaction_amount")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(x => x.TransactionDate)
            .HasColumnName("transaction_date")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.TransactionTime)
            .HasColumnName("transaction_time")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ConfirmedBy)
            .HasColumnName("confirmed_by")
            .HasMaxLength(150)
            .IsRequired(false);

        // Configurar campos de auditoría (heredados de AuditableEntity)
        builder.Property(x => x.CreatedBy)
            .HasColumnName("createdBy")
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("createdAt")
            .HasColumnType("datetime");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("updatedBy")
            .HasMaxLength(255);

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasColumnType("datetime");

        builder.HasIndex(x => x.UniqueId)
            .IsUnique()
            .HasDatabaseName("idx_transfer_validation_unique_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("idx_transfer_validation_status");

        builder.HasIndex(x => new { x.TransactionDate, x.CustomerName })
            .HasDatabaseName("idx_transfer_validation_date_customer");
    }
}
