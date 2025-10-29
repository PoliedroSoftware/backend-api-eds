namespace Poliedro.Eds.Domain.Court.Entities;

public class DocumentEntity
{
    public int IdCourtDocument { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? DocumentName { get; set; }
    public int IdCourt { get; set; }
}
