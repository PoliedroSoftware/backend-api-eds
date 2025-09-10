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
        builder.Property(x => x.BusinessName).HasColumnName("business_name");
        builder.Property(x => x.EdsName).HasColumnName("eds_name");
        builder.Property(x => x.TankNumber).HasColumnName("tank_number");
        builder.Property(x => x.CompartimentNumber).HasColumnName("compartiment_number");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.ProductName).HasColumnName("product_name");
        builder.Property(x => x.ProductDate).HasColumnName("product_date");
    }
}
