using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Dispensers.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class DispensersConfiguration
{
    public DispensersConfiguration(EntityTypeBuilder<DispensersEntity> builder)
    {
        builder.ToTable("dispensers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id_dispensers");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.DispenserTypeId).HasColumnName("id_dispenser_type");
        builder.Property(x => x.EdsId).HasColumnName("id_eds");
        builder.Property(x => x.IdIsland).HasColumnName("idisland");
        builder.Property(x => x.HoseNumber).HasColumnName("number_hose");
    }
}
