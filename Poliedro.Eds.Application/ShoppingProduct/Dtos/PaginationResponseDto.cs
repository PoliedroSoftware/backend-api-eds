namespace Poliedro.Eds.Application.ShoppingProduct.Dtos;
    public class PaginationResponseShoppingProductDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }

