namespace Poliedro.Eds.Domain.Bank.ValueObjects;

public static class BankMovementType
{
    public const string CORTE = "CORTE";
    public const string RETIRO = "RETIRO";
    public const string DEPOSITO = "DEPOSITO";

    private static readonly string[] ValidTypes = { CORTE, RETIRO, DEPOSITO };

    public static bool IsValid(string type)
    {
        return ValidTypes.Contains(type?.ToUpperInvariant());
    }

    public static string[] GetValidTypes()
    {
        return ValidTypes;
    }
}
