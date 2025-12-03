using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class PaymentEntity
{
    public int PaymentFormId { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentDueDate { get; set; }
    public string DurationMeasure { get; set; }
}
