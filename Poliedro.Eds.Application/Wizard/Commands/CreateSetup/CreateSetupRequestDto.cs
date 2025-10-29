using Autofac.Features.Metadata;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.CodeAnalysis;
using Org.BouncyCastle.Utilities;
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

namespace Poliedro.Eds.Application.Wizard.Commands.CreateSetup;

public record CreateSetupRequestDto(
    BusinessDto Bussiness,
    List<EdsDto> EDS,
    List<IslandDto> Islands,
    List<TankDto> Tanks,
    List<CompartimentDto> Compartiments,
    List<DispensersDto> Dispensers,
    List<HoseDto> Hoses,
    List<ProductDto> Products,
    List<IslanderDto> Islanders,
    List<ProviderDto> Providers
    );
