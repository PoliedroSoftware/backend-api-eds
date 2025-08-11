using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainService.Impl
{
    public class BusinessQueryService : IBusinessQueryService
    {
        public Task<BusinessEntity> GetByIdAsync(int idBusiness)
        {
            throw new NotImplementedException();
        }
    }
}
