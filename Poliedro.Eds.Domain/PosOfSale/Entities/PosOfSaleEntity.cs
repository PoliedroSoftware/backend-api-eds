using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.PosOfSaleDetails.Entities;

namespace Poliedro.Eds.Domain.PointOfSale.Entities
{
    public class PosOfSaleEntity : AuditableEntity
    {
        [Key]
        public int IdPos { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ExternalUuid { get; set; }
        public string? Cufe { get; set; }
        public string Status { get; set; } = "SIN_EMITIR";

        public DateTime? IssueDatetime { get; set; }
        public DateTime? DueDate { get; set; }

        public string? IssuerName { get; set; }
        public string? IssuerNit { get; set; }
        public string? IssuerEmail { get; set; }
        public string? IssuerPhone { get; set; }
        public string? IssuerAddress { get; set; }

        public string? BuyerName { get; set; }
        public string? BuyerId { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }
        public string? BuyerAddress { get; set; }

        public string CurrencyCode { get; set; } = "COP";
        public decimal SubtotalAmount { get; set; } = 0;
        public decimal TaxBaseAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;

        public string? PaymentMethod { get; set; }
        public string? PurchaseOrderRef { get; set; }
        public string? Notes { get; set; }

        public string? PdfUrl { get; set; }
        public string? XmlUrl { get; set; }

        public string? WhatsappPhone { get; set; }

        public int? EdsId { get; set; }
        public int? IsleroId { get; set; }
        public string ProviderTag { get; set; } = "PLEMSI";
        [NotMapped]
        public List<PosOfSaleDetailsEntity>? Details { get; set; } = new List<PosOfSaleDetailsEntity>();
    }
}
