using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Poliedro.Eds.Domain.BilligEds.Entities.AllowanceChargeEntities;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class ItemElectronicEntity
{
    public int UnitMeasureId { get; set; }
    public double LineExtensionAmount { get; set; }
    public double? LineDiscountAmount { get; set; }
    public string? LineDiscountType { get; set; }
    public double? unitPriceBeforeDiscount {  get; set; }
    public int? Transaccion { get; set; }

    public bool FreeOfChargeIndicator { get; set; }

    public List<AllowanceChargeEntity>? AllowanceCharges { get; set; }

    public List<TaxTotalEntity>? TaxTotals { get; set; }

    public List<WhitHoldingTaxTotalEntity>? WithHoldingTaxTotal { get; set; }

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public int? Code { get; set; }

    public int TypeItemIdentificationId { get; set; }

    public double PriceAmount { get; set; }

    public double BaseQuantity { get; set; }

    public double InvoicedQuantity { get; set; }

    public double Percent { get; set; }

    public double TaxAmount { get; set; }

    public double UnitPrice { get; set; }

    public double Subtotal { get; set; }
}
