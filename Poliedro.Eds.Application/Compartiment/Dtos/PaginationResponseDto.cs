namespace Poliedro.Eds.Application.Compartiment.Dtos
{
    public class PaginationResponseCompartimentDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}
