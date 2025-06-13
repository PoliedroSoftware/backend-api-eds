using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    internal class CourtExpendituresConfiguration
    {
        public CourtExpendituresConfiguration(EntityTypeBuilder<CourtExpenditureEntity> builder)
        {
            builder.ToTable("court_expenditures");
            builder.HasKey(x => x.IdCourtExpenditure);
            builder.Property(x => x.IdCourtExpenditure).HasColumnName("id_court_expenditure");
            builder.Property(x => x.IdCourt).HasColumnName("id_court");
            builder.Property(x => x.IdExpenditures).HasColumnName("id_expenditures");
            builder.Property(x => x.Amount).HasColumnName("amount");
            builder.Property(x => x.Description).HasColumnName("decription");

            builder.HasOne<CourtEntity>()
                .WithMany(x => x.CourtExpenditures)
                .HasForeignKey(x => x.IdCourt);
        }
    }
}
