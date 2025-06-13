using MediatR;
using Poliedro.Eds.Application.Translations.Dtos;

namespace Poliedro.Eds.Application.Translations.Querys;

public record GetTranslationsQuery() : IRequest<TranslationsAvailableDto>;
