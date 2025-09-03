using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Dtos;

public class StrongBoxDtoCreateRequest
{
    public DateTime DateTime { get; set; }

    public long? IdCorte { get; set; }

    public string Type { get; set; }

    public decimal Ammount { get; set; }

    public string? Note { get; set; }
}
