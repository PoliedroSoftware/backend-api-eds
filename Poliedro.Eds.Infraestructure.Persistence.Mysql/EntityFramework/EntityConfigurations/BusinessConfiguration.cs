using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class BusinessConfiguration
{
    public BusinessConfiguration(EntityTypeBuilder<BusinessEntity> builder)
    {
        builder.ToTable("business");
        builder.HasKey(x => x.IdBusiness);
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}
