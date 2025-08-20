namespace Poliedro.Eds.Application.DashboardPowerBI.Dtos;

public record TypeOfCollectionDto
(
    string IdTypeOfCollection,
    string IdBusiness,
    string IdProduct,
    string Description,
    DateOnly Date
    );
