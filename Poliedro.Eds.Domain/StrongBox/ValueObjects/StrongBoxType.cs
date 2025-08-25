using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.StrongBox.ValueObjects
{
    public static class StrongBoxType
    {
        public const string CORTE = "CORTE";
        public const string RETIRO = "RETIRO";

        public static bool IsValid(string value) => !string.IsNullOrWhiteSpace(value) && (value.Trim().ToUpperInvariant() is CORTE or RETIRO);
    }
}
