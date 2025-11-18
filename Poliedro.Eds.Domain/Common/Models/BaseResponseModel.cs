namespace Poliedro.Eds.Domain.Common.Models;

public class BaseResponseModel
{
    public int StatusCode { get; set; }
    public bool Success { get; set; } = false;
    public string? Message { get; set; } = null;
    public object? Data { get; set; } = null;
}
