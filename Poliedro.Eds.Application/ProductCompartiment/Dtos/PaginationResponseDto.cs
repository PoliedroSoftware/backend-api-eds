namespace Poliedro.Eds.Application.ProductCompartiment.Dtos
{
    public class PaginationResponseDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}
