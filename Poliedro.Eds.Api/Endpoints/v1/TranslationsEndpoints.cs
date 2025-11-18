using MediatR;
using Poliedro.Eds.Application.Translations.Dtos;
using Poliedro.Eds.Application.Translations.Querys;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class TranslationsEndpoints
{
    public static IEndpointRouteBuilder MapTranslationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/translations")
            .WithTags("Translations")
            .AllowAnonymous();

        group.MapGet("", GetAllTranslations).WithName("GetAllTranslations").WithSummary("Get all translations");

        return app;
    }

    private static async Task<TranslationsAvailableDto> GetAllTranslations(IMediator mediator)
    {
        return await mediator.Send(new GetTranslationsQuery());
    }
}
