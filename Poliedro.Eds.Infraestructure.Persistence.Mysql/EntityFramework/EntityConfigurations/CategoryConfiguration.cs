using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class CategoryConfiguration
{
    public CategoryConfiguration(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("category");
        builder.HasKey(x => x.IdCategory);
        builder.Property(x => x.IdCategory).HasColumnName("idcategory");
        builder.Property(x => x.Description).HasColumnName("description");
    }
}