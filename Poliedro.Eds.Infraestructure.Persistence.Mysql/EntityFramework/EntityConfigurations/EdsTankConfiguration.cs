using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.EdsTank.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class EdsTankConfiguration
{
    public EdsTankConfiguration(EntityTypeBuilder<EdsTankEntity> builder)
    {
        builder.ToTable("eds_tank");
        builder.HasKey(x => x.IdEdsTank);
        builder.Property(x => x.IdEdsTank).HasColumnName("id_eds_tank");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
    }
}
