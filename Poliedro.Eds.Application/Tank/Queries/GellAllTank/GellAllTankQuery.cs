using MediatR;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Tank.Queries.GellAllTank;

public record GellAllTankQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<TankDto>>;
