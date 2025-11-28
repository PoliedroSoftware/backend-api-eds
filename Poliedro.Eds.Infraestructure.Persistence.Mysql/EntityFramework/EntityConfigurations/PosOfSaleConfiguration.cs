using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.PointOfSale.Entities;


namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class PosOfSaleConfiguration
{
    public PosOfSaleConfiguration(EntityTypeBuilder<PosOfSaleEntity> builder)
    {
        builder.ToTable("pos_of_sale");
        builder.HasKey(x => x.IdPos);

        builder.Property(x => x.IdPos).HasColumnName("id_pos");
        builder.Property(x => x.InvoiceNumber).HasColumnName("invoice_number");
        builder.Property(x => x.ExternalUuid).HasColumnName("external_uuid");
        builder.Property(x => x.Cufe).HasColumnName("cufe");
        builder.Property(x => x.Status).HasColumnName("status");

        builder.Property(x => x.IssueDatetime).HasColumnName("issue_datetime");
        builder.Property(x => x.DueDate).HasColumnName("due_date");

        builder.Property(x => x.IssuerName).HasColumnName("issuer_name");
        builder.Property(x => x.IssuerNit).HasColumnName("issuer_nit");
        builder.Property(x => x.IssuerEmail).HasColumnName("issuer_email");
        builder.Property(x => x.IssuerPhone).HasColumnName("issuer_phone");
        builder.Property(x => x.IssuerAddress).HasColumnName("issuer_address");

        builder.Property(x => x.BuyerName).HasColumnName("buyer_name");
        builder.Property(x => x.BuyerId).HasColumnName("buyer_id");
        builder.Property(x => x.BuyerEmail).HasColumnName("buyer_email");
        builder.Property(x => x.BuyerPhone).HasColumnName("buyer_phone");
        builder.Property(x => x.BuyerAddress).HasColumnName("buyer_address");

        builder.Property(x => x.CurrencyCode).HasColumnName("currency_code");
        builder.Property(x => x.SubtotalAmount).HasColumnName("subtotal_amount");
        builder.Property(x => x.TaxBaseAmount).HasColumnName("tax_base_amount");
        builder.Property(x => x.TotalAmount).HasColumnName("total_amount");
        builder.Property(x => x.DiscountAmount).HasColumnName("discount_amount");
        builder.Property(x => x.TaxAmount).HasColumnName("tax_amount");

        builder.Property(x => x.PaymentMethod).HasColumnName("payment_method");
        builder.Property(x => x.PurchaseOrderRef).HasColumnName("purchase_order_ref");
        builder.Property(x => x.Notes).HasColumnName("notes");

        builder.Property(x => x.PdfUrl).HasColumnName("pdf_url");
        builder.Property(x => x.XmlUrl).HasColumnName("xml_url");

        builder.Property(x => x.WhatsappPhone).HasColumnName("whatsapp_phone");

        builder.Property(x => x.EdsId).HasColumnName("eds_id");
        builder.Property(x => x.IsleroId).HasColumnName("islero_id");
        builder.Property(x => x.ProviderTag).HasColumnName("provider_tag");

        builder.Property(x => x.CreatedBy).HasColumnName("created_by").HasMaxLength(255);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedBy).HasColumnName("updated_by").HasMaxLength(255);
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
