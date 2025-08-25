using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    public class StrongBoxConfiguration
    {
        public StrongBoxConfiguration(EntityTypeBuilder<StrongBoxEntity> entity)
        {
            entity.ToTable("strongbox");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.DateTime).HasColumnName("datetime");
            entity.Property(x => x.IdCorte).HasColumnName("id_corte");
            entity.Property(x => x.Type).HasColumnName("moviment").HasMaxLength(10);
            entity.Property(x => x.Ammount).HasColumnName("ammount").HasPrecision(18, 2);
            entity.Property(x => x.Saldo).HasColumnName("saldo").HasPrecision(18, 2);
            entity.Property(x => x.Note).HasColumnName("note").HasMaxLength(500);
            entity.Property(x => x.CreatedBy).HasColumnName("createdBy").HasMaxLength(50);
            entity.Property(x => x.CreatedAt).HasColumnName("createdAt");
            entity.Property(x => x.UpdatedBy).HasColumnName("updatedBy").HasMaxLength(100);
            entity.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
        }
    }
}
