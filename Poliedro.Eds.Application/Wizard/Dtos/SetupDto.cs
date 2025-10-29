using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Application.Dispensers.Dtos;
using Poliedro.Eds.Application.Eds.Dtos;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Wizard.Dtos;

public class SetupDto
{
    public BusinessDto Bussiness { get; set; }
    public List<EdsDto> EDS { get; set; } = new List<EdsDto>();
    public List<IslandDto> Islands { get; set; } = new List<IslandDto>();
    public List<TankDto> Tanks { get; set; } = new List<TankDto>();
    public List<CompartimentDto> Compartiments { get; set; } = new List<CompartimentDto>();
    public List<DispensersDto> Dispensers { get; set; } = new List<DispensersDto>();
    public List<HoseDto> Hoses { get; set; } = new List<HoseDto>();
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    public List<IslanderDto> Islanders { get; set; } = new List<IslanderDto>();
    public List<ProviderDto> Providers { get; set; } = new List<ProviderDto>();
}
