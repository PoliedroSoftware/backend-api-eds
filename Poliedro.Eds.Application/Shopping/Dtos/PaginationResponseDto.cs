namespace Poliedro.Eds.Application.Shopping.Dtos;
    public class PaginationResponseShoppingDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }

