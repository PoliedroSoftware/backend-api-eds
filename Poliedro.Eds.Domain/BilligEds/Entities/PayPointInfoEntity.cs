using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class PayPointInfoEntity
{
    public string Code { get; set; }
    public string Address { get; set; }
    public string CashierName { get; set; }
    public string PayPointType { get; set; }
    public string SaleCode { get; set; }

}
