using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class CapacityViewConfiguration
{
    public CapacityViewConfiguration(EntityTypeBuilder<CapacityViewEntity> builder)
    {
        builder.ToTable("v_capacity");
        builder.HasKey(x => x.IdCapacity);
        builder.Property(x => x.IdCapacity).HasColumnName("id_capacity");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Height).HasColumnName("Height");
        builder.Property(x => x.Gallon).HasColumnName("Gallon");
        builder.Property(x => x.Liters).HasColumnName("Liters");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}

