using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl
{
    public class HoseViewDto
    {
        public object IdHose { get; internal set; }
        public object HoseNumber { get; internal set; }
        public object Dispenser { get; internal set; }
        public object AccumulatedGallons { get; internal set; }
        public object AccumulatedAmount { get; internal set; }
        public object ProductType { get; internal set; }
        public object Price { get; internal set; }
        public object IdDispenser { get; internal set; }
        public object Code { get; internal set; }
        public object Number { get; internal set; }
        public object IdDispenserType { get; internal set; }
        public object IdEds { get; internal set; }
        public object IdIsland { get; internal set; }
        public object NumberHose { get; internal set; }
        public object IdProductType { get; internal set; }
        public object Description { get; internal set; }
        public object EdsId { get; internal set; }
        public object Name { get; internal set; }
        public object Nit { get; internal set; }
        public object Address { get; internal set; }
        public object Sicom { get; internal set; }
        public object IdBusiness { get; internal set; }
    }
}