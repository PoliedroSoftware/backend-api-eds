using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory;

public record CreateCourtDispensersInventoryCommand(CreateCourtDispensersInventoryRequestDto Request) : IRequest<Result<VoidResult, Error>>;
