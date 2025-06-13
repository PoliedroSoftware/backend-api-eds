namespace Poliedro.Eds.Application.Court.Dtos
{
    public class CourtDispenserDto
    {
        public int IdCourtDispensers { get; set; }
        public int IdCourt { get; set; }
        public double AccumulatedAmount { get; set; }
        public double AccumulatedGallons { get; set; }
        public int IdProduct { get; set; }
        public int IdCompartiment { get; set; }
        public int IdHose { get; set; }
        //public ICollection<CourtEntity> Courts { get; set; }
        //public ICollection<ProductEntity> Products { get; set; }
        //public ICollection<CompartimentEntity> Compartiments { get; set; }
    }
}
