using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class GeneralAllowanceEntity
{
    public string? AllowanceChargeReason { get; set; }
    public double? AllowancePercent { get; set; }
    public required decimal Amount { get; set; }
    public required decimal BaseAmount { get; set; }
}
