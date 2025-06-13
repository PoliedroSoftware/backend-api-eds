namespace Poliedro.Eds.Application.Court.Dtos
{
    public class CourtExpenditureDto
    {
        public int IdCourtExpenditure { get; set; }
        public int IdCourt { get; set; }
        public int IdExpenditures { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
    }
}
