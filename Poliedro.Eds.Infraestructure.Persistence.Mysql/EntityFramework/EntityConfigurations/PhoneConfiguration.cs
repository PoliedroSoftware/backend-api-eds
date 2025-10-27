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
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        
        // Audit fields configuration
        builder.Property(x => x.CreatedBy).HasColumnName("createdBy");
        builder.Property(x => x.CreatedAt).HasColumnName("createdAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("updatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
    }
}
