using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;

public class ProviderViewConfiguration
{
    public ProviderViewConfiguration(EntityTypeBuilder<ProviderViewEntity> builder)
    {
        builder.ToTable("v_providers");
        builder.HasKey(x => x.IdProvider);
        builder.Property(x => x.IdProvider).HasColumnName("id_provider");
        builder.Property(x => x.IdProduct).HasColumnName("id_product");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Date).HasColumnName("date");
    }
}
