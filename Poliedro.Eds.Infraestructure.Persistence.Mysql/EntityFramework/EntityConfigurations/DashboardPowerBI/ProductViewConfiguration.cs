using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class ProductViewConfiguration
{
    public ProductViewConfiguration(EntityTypeBuilder<ProductViewEntity> builder)
    {
        builder.ToTable("v_product");
        builder.HasKey(x => x.IdProduct);
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IdProductType).HasColumnName("id_product_type");
        builder.Property(x => x.Price).HasColumnName("price");
        builder.Property(x => x.Date).HasColumnName("date");

    }
}
