using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.RegisterShift.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class RegisterShiftConfiguration
{
    public RegisterShiftConfiguration(EntityTypeBuilder<RegisterShiftEntity> builder)
    {
        builder.ToTable("registershift");
        builder.HasKey(x => x.IdRegisterShift);

        builder.Property(x => x.IdRegisterShift).HasColumnName("id_registershift");
        builder.Property(x => x.IdEds).HasColumnName("id_eds");
        builder.Property(x => x.IdBusiness).HasColumnName("id_business");
        builder.Property(x => x.IdIslander).HasColumnName("id_islander");
        builder.Property(x => x.DateStartTime).HasColumnName("date_starttime");
        builder.Property(x => x.StartTime).HasColumnName("starttime");
        builder.Property(x => x.DateEndTime).HasColumnName("date_endtime");
        builder.Property(x => x.EndTime).HasColumnName("endtime");
    }
}
