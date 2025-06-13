namespace Poliedro.Eds.Domain.Court.Entities
{
    public class DispenserEntity
    {
        public int IdCourtDispensers { get; set; }
        public int IdCourt { get; set; }
        public double AccomulatedAmount { get; set; }
        public double AccomulatedGallons { get; set; }
        public int IdProdut { get; set; }
        public int IdCompartiment { get; set; }
        public int IdHose { get; set; }
    }
}
