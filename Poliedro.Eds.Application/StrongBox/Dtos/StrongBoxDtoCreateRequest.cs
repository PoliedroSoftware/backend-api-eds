using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Dtos;

public record StrongBoxDtoCreateRequest(
    DateTime DateTime,
    long? IdCorte,
    string Type,
    decimal Ammount,
    string? Note
);
