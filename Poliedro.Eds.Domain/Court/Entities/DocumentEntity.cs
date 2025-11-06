namespace Poliedro.Eds.Domain.Court.Entities;

public class DocumentEntity
{
    public int IdCourtDocument { get; set; }
    // Hacer nullable para evitar guardar imágenes base64 pesadas
    public string? Descripcion { get; set; } = null;
    public string? DocumentName { get; set; }
    public int IdCourt { get; set; }
}
