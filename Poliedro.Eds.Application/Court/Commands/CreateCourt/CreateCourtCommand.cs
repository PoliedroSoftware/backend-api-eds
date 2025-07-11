using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Court.Commands.CreateCourt;

public record CreateCourtCommand(
    int IdBusiness,
    int IdEds,
    int IdIslander,
    DateOnly DateStarttime,
    TimeOnly Starttime,
    DateOnly DateEndtime,
    TimeOnly Endtime,
    string? Descripcion,
    double? Distintic,
    IEnumerable<CourtDispenserCommand> CourtDispensers,
    IEnumerable<DocumentCommand> CourtDocuments,
    IEnumerable<CourtExpenditureCommand> CourtExpenditures,
    IEnumerable<CourtTypeOfCollectionCommand> CourtTypeOfCollections
    ) : IRequest<Result<VoidResult, Error>>;

