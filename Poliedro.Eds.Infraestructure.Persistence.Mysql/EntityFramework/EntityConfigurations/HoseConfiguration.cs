using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Hose.Entities;
using System.Reflection.Emit;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class HoseConfiguration
{
    public HoseConfiguration(EntityTypeBuilder<HoseEntity> builder)
    {
        builder.ToTable("hose");
        builder.HasKey(x => x.IdHose);
        builder.Property(x => x.IdHose).HasColumnName("id_hose");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.AccumulatedAmount).HasColumnName("accumulated_amount");
        builder.Property(x => x.AccumulatedGallons).HasColumnName("accumulated_gallons");
        builder.Property(x => x.IdDispensers).HasColumnName("id_dispensers");
        builder.Property(x => x.IdProductType).HasColumnName("id_product_type");
    }
}
