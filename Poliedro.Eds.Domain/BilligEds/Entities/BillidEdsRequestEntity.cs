using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class BillidEdsRequestEntity
{
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
    public Attachment2Entity Attachment2 { get; set; }
    public CustomerEntity CustomerEntity { get; set; }

};
