using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.RegisterShift.Entities;

public class RegisterShiftEntity : AuditableEntity
{
    [Key]
    public int IdRegisterShift { get; set; }
    public int IdEds { get; set; }
    public DateOnly DateStartTime { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateOnly DateEndTime { get; set; }
    public TimeOnly EndTime { get; set; }
}


