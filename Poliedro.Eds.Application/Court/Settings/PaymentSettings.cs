namespace Poliedro.Eds.Application.Court.Settings;

/// <summary>
/// Configuración de medios de pago para el sistema de cortes.
/// </summary>
public class PaymentSettings
{
    /// <summary>
    /// Lista de nombres de medios de pago que se consideran bancarios
  /// y deben registrarse en el sistema de banco.
  /// </summary>
    public string[] BancaryPaymentMethods { get; set; } = Array.Empty<string>();
}
