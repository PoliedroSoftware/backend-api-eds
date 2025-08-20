using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class BusinessViewConfiguration
{
    public BusinessViewConfiguration(EntityTypeBuilder<BusinessViewEntity> builder)
    {
        builder.ToTable("v_business");
        builder.HasKey(x => x.IdBusiness);
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IdTank).HasColumnName("tank_number");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdCompartiment).HasColumnName("compartiment");
        builder.Property(x => x.NameEds).HasColumnName("eds");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}


