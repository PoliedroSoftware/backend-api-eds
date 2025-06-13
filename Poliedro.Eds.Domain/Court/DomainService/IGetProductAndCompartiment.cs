using Poliedro.Eds.Domain.Court.Dto;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetProductAndCompartiment
{
    Task<ProductAndCompartimentDto> GetProductAndCompartimentAsync(int hoseid);
}
