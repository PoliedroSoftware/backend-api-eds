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
    public RegisterShiftConfiguration(EntityTypeBuilder<RegisterShiftEntity> entity)
    {
        entity.ToTable("registershift");
        entity.HasKey(x => x.IdRegisterShift);

        entity.Property(x => x.IdRegisterShift).HasColumnName("id_registershift");
        entity.Property(x => x.IdEds).HasColumnName("id_eds");
        entity.Property(x => x.DateStartTime).HasColumnName("date_starttime");
        entity.Property(x => x.StartTime).HasColumnName("starttime");
        entity.Property(x => x.DateEndTime).HasColumnName("date_endtime");
        entity.Property(x => x.EndTime).HasColumnName("endtime");
    }
}
