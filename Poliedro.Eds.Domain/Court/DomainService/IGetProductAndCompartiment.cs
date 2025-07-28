using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Domain.Court.DomainService;

public interface IGetProductAndCompartiment
{
    Task<ProductAndCompartimentEntity> GetProductAndCompartimentAsync(int hoseid);
}
