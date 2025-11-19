using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentDashboardView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class CompartimentDashboardViewConfiguration
{
    public CompartimentDashboardViewConfiguration(EntityTypeBuilder<CompartimentDashboardViewEntity> builder)
    {
        builder.ToTable("v_d_compartiment");
        builder.HasKey(x => x.IdCompartment);
        builder.Property(x => x.IdCompartment).HasColumnName("id_compartiment");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.Nominal).HasColumnName("nominal");
        builder.Property(x => x.Operative).HasColumnName("operative");
        builder.Property(x => x.Height).HasColumnName("height");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.ProductName).HasColumnName("product_name");
        builder.Property(x => x.IdTank).HasColumnName("id_tank");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
