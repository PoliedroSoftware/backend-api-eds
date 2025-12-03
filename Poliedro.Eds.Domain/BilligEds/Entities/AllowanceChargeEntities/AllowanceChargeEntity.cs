using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities.AllowanceChargeEntities;

public class AllowanceChargeEntity
{
    public bool ChargeIndicator { get; set; }
    public string? AllowanceChargeReason { get; set; }
    public decimal MultiplierFactorNumeric { get; set; }
    public decimal Amount {  get; set; }
    public decimal BaseAmount { get; set; }
}
