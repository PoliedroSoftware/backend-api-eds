using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class PhoneConfiguration
{
    public PhoneConfiguration(EntityTypeBuilder<PhoneEntity> builder)
    {
        builder.ToTable("phone");
        builder.HasKey(x => x.IdPhone);
        builder.Property(x => x.IdPhone).HasColumnName("id_phone");
        builder.Property(x => x.Number).HasColumnName("number").HasMaxLength(13).IsRequired();
    }
}
