namespace Poliedro.Eds.Application.DispenserType.Dtos
{
    public class PaginationResponseDispenserTypeDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}
