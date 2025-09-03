using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Dtos;

public class StrongBoxTotalBalanceDto
{
    public long? Id { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal Saldo { get; set; }
}
