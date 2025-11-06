namespace Poliedro.Eds.Application.Court.Dtos
{
    public class DocumentDto
    {
        public int IdCourtDocument { get; set; }
        // Hacer nullable para evitar guardar imágenes base64 pesadas
        public string? Descripcion { get; set; } = null;
        public string? DocumentName { get; set; }
        public int IdCourt { get; set; }
    }
}
