using MediatR;
using Poliedro.Eds.Application.Tank.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Tank.Queries.GetTankById;
public record GetTankByIdQuery(int Id) : IRequest<Result<TankDto, Error>>;
