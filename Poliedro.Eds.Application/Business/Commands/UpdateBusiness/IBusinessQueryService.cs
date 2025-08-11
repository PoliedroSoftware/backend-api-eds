using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness
{
    public interface IBusinessQueryService
    {
        Task<BusinessEntity> GetByIdAsync(int idBusiness);
    }
}
