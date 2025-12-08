using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSale.Commands.UpdatePosOfSale;

public record UpdatePosOfSaleCommand(
     int IdPos,
     string InvoiceNumber,
     string? ExternalUuid,
     string? Cufe,
     string? Status,
     DateTime? IssueDatetime,
     DateTime? DueDate,
     string? IssuerName,
     string? IssuerNit,
     string? IssuerEmail,
     string? IssuerPhone,
     string? IssuerAddress,
     string? BuyerName,
     string? BuyerId,
     string? BuyerEmail,
     string? BuyerPhone,
     string? BuyerAddress,
     string? CurrencyCode,
     decimal? SubtotalAmount,
     decimal? TaxBaseAmount,
     decimal? TotalAmount,
     decimal? DiscountAmount,
     decimal? TaxAmount,
     string? PaymentMethod,
     string? PurchaseOrderRef,
     string? Notes,
     string? PdfUrl,
     string? XmlUrl,
     string? WhatsappPhone,
     int? EdsId,
     int? IsleroId,
     string? ProviderTag) : IRequest<Result<VoidResult, Error>>;
