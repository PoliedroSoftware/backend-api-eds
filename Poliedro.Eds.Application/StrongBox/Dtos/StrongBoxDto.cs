using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Dtos
{
    public record StrongBoxDto(
        long Id,
        DateTime DateTime,
        long? IdCorte,
        string Type,
        decimal Ammount,
        decimal Saldo,
        string? Note,
        string CreatedBy,
        DateTime CreatedAt,
        string? UpdatedBy,
        DateTime? UpdatedAt
    );

    public record StrongBoxCreateRequest(
        DateTime DateTime,
        long? IdCorte,
        string Type,
        decimal Ammount,
        string? Note,
        string CreatedBy
    );
}
