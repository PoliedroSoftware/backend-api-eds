using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Islander.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class IslanderConfiguration
{
    public IslanderConfiguration(EntityTypeBuilder<IslanderEntity> builder)
    {
        builder.ToTable("islander");
        builder.HasKey(x => x.IdIslander);
        builder.Property(x => x.IdIslander).HasColumnName("id_islander");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.FirstName).HasColumnName("first_name");
        builder.Property(x => x.LastName).HasColumnName("last_name");

    }
}
