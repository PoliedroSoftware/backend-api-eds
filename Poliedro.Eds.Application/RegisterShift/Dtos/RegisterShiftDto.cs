using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.RegisterShift.Dtos;

public class RegisterShiftDto
{
    public int IdRegisterShift { get; set; }
    public int IdEds { get; set; }
    public DateOnly DateStartTime { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateOnly? DateEndTime { get; set; }
    public TimeOnly? EndTime { get; set; }

}
