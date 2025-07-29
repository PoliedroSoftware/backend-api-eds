using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class EdsViewConfiguration
{
    public EdsViewConfiguration(EntityTypeBuilder<EdsViewEntity> builder)
    {
        builder.ToTable("v_eds");
        builder.HasKey(x => x.IdEds);
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Nit).HasColumnName("nit");
        builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.Sicom).HasColumnName("sicom");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
