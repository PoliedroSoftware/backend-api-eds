namespace Poliedro.Eds.Application.Court.Dtos
{
    public class CourtTypeOfCollectionDto
    {
        public int IdCourtTypeOfCollection { get; set; }
        public int IdCourt { get; set; }
        public int IdTypeOfCollection { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
