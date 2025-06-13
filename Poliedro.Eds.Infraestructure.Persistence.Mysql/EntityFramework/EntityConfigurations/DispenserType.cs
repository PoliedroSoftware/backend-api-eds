using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DispenserType.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class DispenserTypeConfiguration
{
    public DispenserTypeConfiguration(EntityTypeBuilder<DispenserTypeEntity> builder)
    {
        builder.ToTable("dispenser_type");
        builder.HasKey(x => x.IdType);
        builder.Property(x => x.IdType).HasColumnName("id_dispenser_type");
        builder.Property(x => x.Description).HasColumnName("description");
    }
}
