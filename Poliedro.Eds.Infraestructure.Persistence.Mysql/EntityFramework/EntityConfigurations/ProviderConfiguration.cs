using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Provider.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ProviderConfiguration
{
    public ProviderConfiguration(EntityTypeBuilder<ProviderEntity> builder)
    {
        builder.ToTable("provider");
        builder.HasKey(x => x.IdProvider);
        builder.Property(x => x.IdProvider).HasColumnName("id_provider");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}
