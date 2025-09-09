using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Dtos;

public class StrongBoxDto
{
    public long Id { get; set; }

    public DateTime DateTime { get; set; }

    public long? IdCorte { get; set; }

    public string Type { get; set; }

    public double Ammount { get; set; }

    public double Saldo { get; set; }

    public string? Note { get; set; }

}
