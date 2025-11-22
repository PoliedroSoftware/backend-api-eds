using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.PointOfSale.Entities;

namespace Poliedro.Eds.Domain.PosOfSaleDetail.Entities
{
    public class PosOfSaleDetailEntity: AuditableEntity
    {
        [Key]
        public ulong IdDetail { get; set; }

        public ulong PosId { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitMeasure { get; set; } = "UND";
        public decimal Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; } = 0;

        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;

        public string? TaxType { get; set; } = "IVA";
        public decimal TaxPercentage { get; set; } = 0;
        public string? RetentionType { get; set; }
        public decimal RetentionAmount { get; set; } = 0;

        public PosOfSaleEntity? PosOfSale { get; set; }
    }
}
