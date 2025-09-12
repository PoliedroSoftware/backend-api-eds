namespace Poliedro.Eds.Domain.Court.Entities
{
    public class CourtDispenserTransactionData
    {
        public double AccumulatedAmount { get; set; }
        public double AccumulatedGallons { get; set; }
        public double LastAccumulatedAmount { get; set; }
        public double LastAccumulatedGallons { get; set; }
        public double AmountDifferenceResult { get; set; }
        public double GallonsDifferenceResult { get; set; }
        public int IdHose { get; set; }
        public int NumberName { get; set; }
        public int DispenserNumber { get; set; }
    }
}
