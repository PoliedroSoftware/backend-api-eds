using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Court.Commands.UpdateCourt
{
    public record UpdateCourtCommand(int IdCourt,
    int IdIslander,
    DateOnly DateStarttime,
    TimeOnly Starttime,
    DateOnly DateEndtime,
    TimeOnly Endtime,
    int Consecutive,
    int IdEds,
    string? Descripcion,
    double Distintic,
    IEnumerable<CourtDispenserDto> CourtDispensers,
    IEnumerable<DocumentDto> CourtDocuments,
    IEnumerable<CourtExpenditureDto> CourtExpenditures,
    IEnumerable<CourtTypeOfCollectionDto> CourtTypeOfCollections
    ) : IRequest<Result<VoidResult, Error>>;
    public record GetCourtByIdCommand(int Id) : IRequest<Result<CourtDto, Error>>;
}
