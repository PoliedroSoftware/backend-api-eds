namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos.Court
{
    public class CourtExpenditureDto
    {
        public string IdCourtExpenditure { get; set; }
        public string IdCourt { get; set; }
        public string IdExpenditures { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
    }
}
