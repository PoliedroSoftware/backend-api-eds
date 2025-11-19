using Poliedro.Eds.Api.Endpoints.v1;

namespace Poliedro.Eds.Api.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        // Map all v1 endpoints
        app.MapAuthEndpoints();
        app.MapAccountEndpoints();
        app.MapBankEndpoints();
        app.MapBusinessEndpoints();
        app.MapCapacityEndpoints();
        app.MapCategoryEndpoints();
        app.MapCompartimentEndpoints();
        app.MapCompartimentCapacityEndpoints();
        app.MapCourtEndpoints();
        app.MapCourtDispensersInventoryEndpoints();
        app.MapDashboardPowerBIEndpoints();
        app.MapDispensersEndpoints();
        app.MapDispenserTypeEndpoints();
        app.MapEdsEndpoints();
        app.MapEdsTankEndpoints();
        app.MapExpendituresEndpoints();
        app.MapFileUploadS3Endpoints();
        app.MapHoseEndpoints();
        app.MapHoseHistoryEndpoints();
        app.MapInventoryEndpoints();
        app.MapIoTEndpoints();
        app.MapIslandEndpoints();
        app.MapIslanderEndpoints();
        app.MapOpenAIEndpoints();
        app.MapPhoneEndpoints();
        app.MapProductEndpoints();
        app.MapProductTypeEndpoints();
        app.MapProviderEndpoints();
        app.MapRegisterShiftEndpoints();
        app.MapSetupWizardEndpoints();
        app.MapShoppingEndpoints();
        app.MapShoppingProductEndpoints();
        app.MapShoppingProductInventoryEndpoints();
        app.MapStrongBoxEndpoints();
        app.MapTankEndpoints();
        app.MapTransferValidationEndpoints();
        app.MapTranslationsEndpoints();
        app.MapTypeOfCollectionEndpoints();
        app.MapWhatsAppEndpoints();
        
        return app;
    }
}
