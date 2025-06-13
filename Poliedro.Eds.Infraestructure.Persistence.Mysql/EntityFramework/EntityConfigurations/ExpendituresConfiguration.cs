using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ExpendituresConfiguration
{
    public ExpendituresConfiguration(EntityTypeBuilder<ExpendituresEntity> builder)
    {
        builder.ToTable("expenditures");
        builder.HasKey(x => x.IdExpenditures);
        builder.Property(x => x.IdExpenditures).HasColumnName("id_Expenditures");
        builder.Property(x => x.Description).HasColumnName("description");

    }
}
