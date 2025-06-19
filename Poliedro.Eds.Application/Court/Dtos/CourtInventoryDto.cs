namespace Poliedro.Eds.Application.Court.Dtos
{
    public class CourtInventoryDto
    {
        public int IdInventory { get; set; }
        public DateOnly Date { get; set; }
        public string ReferenceType { get; set; }
        public int ReferenceId { get; set; }
    }
}
