using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.Inventory.Dto.View;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Tank.Entities;

namespace Poliedro.Eds.Domain.Wizard.Entities;

public class SetupEntity : AuditableEntity
{
    public BusinessEntity Bussiness { get; set; }
    public List<EdsEntity> EDS { get; set; } = new List<EdsEntity>();
    public List<IslandEntity> Islands { get; set; } = new List<IslandEntity>();
    public List<TankEntity> Tanks { get; set; } = new List<TankEntity>();
    public List<CompartimentEntity> Compartiments { get; set; } = new List<CompartimentEntity>();
    public List<DispensersEntity> Dispensers { get; set; } = new List<DispensersEntity>();
    public List<HoseEntity> Hoses { get; set; } = new List<HoseEntity>();
    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    public List<IslanderEntity> Islanders { get; set; } = new List<IslanderEntity>();
    public List<ProviderEntity> Providers { get; set; } = new List<ProviderEntity>();
}
