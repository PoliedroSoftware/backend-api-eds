using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class CapacityDashboardViewConfiguration
{
    public CapacityDashboardViewConfiguration(EntityTypeBuilder<CapacityDashboardViewEntity> builder)
    {
        builder.ToTable("v_d_capacity");
        builder.HasKey(x => x.IdCapacity);
        builder.Property(x => x.IdCapacity).HasColumnName("id_capacity");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Height).HasColumnName("height");
        builder.Property(x => x.Gallon).HasColumnName("gallon");
        builder.Property(x => x.Liters).HasColumnName("liters");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
