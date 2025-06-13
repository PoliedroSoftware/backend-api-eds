using MediatR;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GetCourtDispensersInventoryById;
public record GetCourtDispensersInventoryByIdQuery(int Id) : IRequest<Result<CourtDispensersInventoryDto, Error>>;
