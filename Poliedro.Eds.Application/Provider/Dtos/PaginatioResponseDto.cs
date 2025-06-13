namespace Poliedro.Eds.Application.Provider.Dtos;

public class PaginationResponseDto<T>
{
    public List<T> Data { get; set; }
    public int TotalPages { get; set; }
    public int TotalRows { get; set; }
}