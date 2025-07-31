using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.Court.Entities.View;

public class CourtSenMessageEntity : CourtEntity
{
    public string IslanderName { get; set; } = string.Empty;
    public string EdsName { get; set; } = string.Empty;
    public string ExpenditureName { get; set; } = string.Empty;
}
