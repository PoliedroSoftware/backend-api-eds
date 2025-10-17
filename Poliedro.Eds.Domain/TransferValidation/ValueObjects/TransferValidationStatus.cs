namespace Poliedro.Eds.Domain.TransferValidation.ValueObjects;

public static class TransferValidationStatus
{
    public const string PENDIENTE = "PENDIENTE";
    public const string CONFIRMADA = "CONFIRMADA";
    public const string RECHAZADA = "RECHAZADA";

    private static readonly string[] ValidStatuses = { PENDIENTE, CONFIRMADA, RECHAZADA };

    public static bool IsValid(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return false;

        var normalized = status.Trim().ToUpperInvariant();
        return ValidStatuses.Contains(normalized);
    }

    public static IEnumerable<string> GetValidStatuses()
    {
        return ValidStatuses;
    }

    public static string Normalize(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("El estado no puede ser vacío", nameof(status));

        var normalized = status.Trim().ToUpperInvariant();
        
        if (!IsValid(normalized))
            throw new ArgumentException($"Estado inválido: {status}", nameof(status));

        return normalized;
    }
}
