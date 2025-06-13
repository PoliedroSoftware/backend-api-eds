namespace Poliedro.Eds.Application.Category.Dtos;
    public class PaginationResponseCategoryDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
    }

