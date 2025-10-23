using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Audit.Entities;

namespace Poliedro.Eds.Domain.RegisterShift.Entities;

public class RegisterShiftEntity : AuditableEntity
{
    public int IdRegisterShift { get; set; } = default!;
    public int IdEds { get; set; }
    public DateOnly DateStartTime { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateOnly? DateEndTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    private RegisterShiftEntity() { }

    public RegisterShiftEntity(
        int idEds,
        DateOnly dateStartTime,
        TimeOnly startTime,
        DateOnly? dateEndTime,
        TimeOnly? endTime)
    {
        if (idEds <= 0)
        {
            throw new ArgumentException("IdEds No debe ser 0", nameof(idEds));
        }
        IdEds = idEds;
        DateStartTime = dateStartTime;
        StartTime = startTime;
        DateEndTime = dateEndTime;
        EndTime = endTime;
    }
}


