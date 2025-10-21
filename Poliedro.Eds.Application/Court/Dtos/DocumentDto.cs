namespace Poliedro.Eds.Application.Court.Dtos
{
    public class DocumentDto
    {
        public int IdCourtDocument { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? DocumentName { get; set; }
        public int IdCourt { get; set; }
    }
}
