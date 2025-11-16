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
        
        // TODO: Add remaining endpoint mappers as they are created
        
        return app;
    }
}
