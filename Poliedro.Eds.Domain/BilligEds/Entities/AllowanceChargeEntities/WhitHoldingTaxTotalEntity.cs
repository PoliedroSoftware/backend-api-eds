using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities.AllowanceChargeEntities;

public class WhitHoldingTaxTotalEntity
{
    public int TaxId { get; set; }
    public int Percent {  get; set; }
    public  int TaxAmount { get; set; }
    public int TaxableAmount { get; set; }
}
