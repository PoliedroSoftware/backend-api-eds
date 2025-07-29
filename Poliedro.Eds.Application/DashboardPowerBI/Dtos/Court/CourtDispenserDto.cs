namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos.Court
{
    public class CourtDispenserDto
    {
        public string IdCourtDispensers { get; set; }

        public string IdCourt { get; set; }

        public double AccumulatedAmount { get; set; }

        public double AccumulatedGallons { get; set; }

        public string IdProduct { get; set; }

        public string IdCompartiment { get; set; }

        public string IdHose { get; set; }
        //public ICollection<CourtEntity> Courts { get; set; }
        //public ICollection<ProductEntity> Products { get; set; }
        //public ICollection<CompartimentEntity> Compartiments { get; set; }
    }
}
