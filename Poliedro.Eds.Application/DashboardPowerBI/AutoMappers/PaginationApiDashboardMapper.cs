

using AutoMapper;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.DashboardPowerBI.Entities.Master;

namespace Poliedro.Eds.Application.DashboardPowerBI.AutoMappers
{
    public class PaginationApiDashboardMapper: Profile
    {
        public PaginationApiDashboardMapper()
        {
            CreateMap<PaginationResponse<MasterEntity>, PaginationResponseDto<MasterDto>>().ReverseMap();
        }
    }
}
