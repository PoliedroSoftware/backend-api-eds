namespace Poliedro.Eds.Application.Court.Dtos
{
    public class CourtDto
    {
        public int IdCourt { get; set; }
        public int IdIslander { get; set; }
        public DateOnly DateStarttime { get; set; }
        public TimeOnly Starttime { get; set; }
        public DateOnly DateEndtime { get; set; }
        public TimeOnly Endtime { get; set; }
        public int Consecutive { get; set; }
        public int IdEds { get; set; }
        public string? Descripcion { get; set; }
        public double Distintic { get; set; }
        public IEnumerable<CourtDispenserDto> CourtDispensers { get; set; }
        public IEnumerable<DocumentDto> CourtDocuments{ get; set; }
        public IEnumerable<CourtExpenditureDto> CourtExpenditures { get; set; }
        public IEnumerable<CourtTypeOfCollectionDto> CourtTypeOfCollections { get; set; }
    }
}
