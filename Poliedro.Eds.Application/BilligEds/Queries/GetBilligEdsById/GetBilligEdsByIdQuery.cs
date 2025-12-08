using MediatR;
using Poliedro.Eds.Application.BilligEds.Dtos;

namespace Poliedro.Eds.Application.BilligEds.Queries.GetBilligEdsById;

public record GetBilligEdsByIdQuery(int Id) : IRequest<BilligEdsDto>;
