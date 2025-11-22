using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.PosOfSaleDetail.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class PosOfSaleDetailConfiguration
{
    public PosOfSaleDetailConfiguration(EntityTypeBuilder<PosOfSaleDetailEntity> builder)
    {
        builder.ToTable("pos_of_sale_details");
        builder.HasKey(x => x.IdDetail);

        builder.Property(x => x.IdDetail).HasColumnName("id_detail");

        builder.Property(x => x.PosId).HasColumnName("pos_id");

        builder.Property(x => x.ProductCode).HasColumnName("product_code");
        builder.Property(x => x.ProductName).HasColumnName("product_name");
        builder.Property(x => x.UnitMeasure).HasColumnName("unit_measure");
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        builder.Property(x => x.UnitPrice).HasColumnName("unit_price");

        builder.Property(x => x.DiscountAmount).HasColumnName("discount_amount");
        builder.Property(x => x.TaxAmount).HasColumnName("tax_amount");
        builder.Property(x => x.TotalAmount).HasColumnName("total_amount");

        builder.Property(x => x.TaxType).HasColumnName("tax_type");
        builder.Property(x => x.TaxPercentage).HasColumnName("tax_percentage");
        builder.Property(x => x.RetentionType).HasColumnName("retention_type");
        builder.Property(x => x.RetentionAmount).HasColumnName("retention_amount");

        builder
            .HasOne(x => x.PosOfSale)
            .WithMany()
            .HasForeignKey(x => x.PosId);
    }
}
