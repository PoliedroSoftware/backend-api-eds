using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class IslandConfiguration
{
    public IslandConfiguration(EntityTypeBuilder<IslandEntity> builder)
    {
        builder.ToTable("island");
        builder.HasKey(x => x.IdIsland);
        builder.Property(x => x.IdIsland).HasColumnName("idisland");
        builder.Property(x => x.Description).HasColumnName("description");
    }
}
