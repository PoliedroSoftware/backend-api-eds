using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Poliedro.Eds.Domain.BilligEds.Entities.AllowanceChargeEntities;

public class AllHoldingsTaxTotalEntity
{
    public int TaxId { get; set; }

    public double TaxAmount { get; set; }

    public double Percent { get; set; }

    public double TaxableAmount { get; set; }
}
