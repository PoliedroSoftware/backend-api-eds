namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos
{
    public class InventoryDto
    {
        public IEnumerable<BusinessDto> Businesses { get; set; }
    }

    public class BusinessDto
    {
        public string IdBusiness { get; set; }
        public string Business { get; set; }
        public IEnumerable<EdssDto> Eds { get; set; }
    }

    public class EdssDto
    {
        public string IdEds { get; set; }
        public string Eds { get; set; }
        public IEnumerable<TankDto> Tanks { get; set; }
    }

    public class TankDto
    {
        public string IdTank { get; set; }
        public string Tank { get; set; }
        public double TankCapacity { get; set; }
        public IEnumerable<CompartmentDto> Compartments { get; set; }
    }

    public class CompartmentDto
    {
        public string IdCompartment { get; set; }
        public int Compartment { get; set; }
        public string IdProduct { get; set; }
        public string Product { get; set; }
        public double Stock { get; set; }
    }
}
