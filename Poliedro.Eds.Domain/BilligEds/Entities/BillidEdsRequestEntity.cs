using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Poliedro.Eds.Domain.BilligEds.Entities.AllowanceChargeEntities;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class BillidEdsRequestEntity
{
    [Key]
    public DateTime Date { get; set;  }
    public DateTime Time { get; set; }
    public string SendToEmail { get; set; }
    public SoftwareManufactureEntity? SoftwareManufacture {  get; set; }
    public PayPointInfoEntity? PayPointInfo { get; set; }
    public string Number {  get; set; }
    public string Prefix { get; set; }
    public DateTime TransactionDate { get; set; }
    public OrderReferenceEntity? OrderReference { get; set; }
    public bool SendEmail { get; set; }
    public Attachment1Entity? Attachment1 { get; set; }
    public Attachment2Entity? Attachment2 { get; set; }
    public CustomerEntity? CustomerEntity { get; set; }
    public PaymentEntity? PaymentEntity { get; set; }
    public List<GeneralAllowanceEntity>? GeneralAllowanceEntities { get; set; }
    public List<ItemElectronicEntity>? ItemElectronicEntities { get; set; }
    public string? Resolution { get; set; }
    public string? ResolutionText { get; set; }
    public string? HeadNote { get; set; }
    public string? FootNote { get; set; }
    public string? Notes { get; set; }
    public decimal? TotalBeforeTax { get; set; }
    public double? DiscountAmountByInvoice { get; set; }
    public string? DiscountType { get; set; }
    public double AllowanceTotal { get; set; }
    public double InvoiceBaseTotal { get; set; }
    public double InvoiceTaxExclusiveTotal { get; set; }
    public double InvoiceTaxInclusiveTotal { get; set; }
    public double TotalToPay { get; set; }
    public List<AllTaxTotalEntity> AllTaxTotalEntities { get; set; }
    public List<AllHoldingsTaxTotalEntity>? allHoldingsTaxTotalEntity {  get; set; }
    public double FinalTotalPay { get; set; }
};
