using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;


namespace Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory;

public record UpdateCourtDispensersInventoryCommand(
    int IdCourtDispensersInventory,
    int IdCourtdDispensers,
    int IdInventory) : IRequest<Result<VoidResult, Error>>;