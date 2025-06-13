using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Eds.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class EdsConfiguration
{
    public EdsConfiguration(EntityTypeBuilder<EdsEntity> builder)
    {
        builder.ToTable("eds");
        builder.HasKey(x => x.IdEds);
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Nit).HasColumnName("nit");
        builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.Sicom).HasColumnName("sicom");
        builder.Property(x => x.IdBusiness).HasColumnName("idbusiness");
    }
}
