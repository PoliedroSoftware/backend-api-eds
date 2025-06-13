namespace Poliedro.Eds.Domain.Common.Pagination
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; } = [];
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }
}
